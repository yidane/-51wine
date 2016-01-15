﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Travel.Application.DomainModules.Order.Core
{
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.Odbc;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Transactions;

    using Travel.Application.DomainModules.Order.Core.Interface;
    using Travel.Application.DomainModules.Order.Entity;
    using Travel.Infrastructure.DomainDataAccess.Order;
    using Travel.Infrastructure.WeiXin.Advanced;
    using Travel.Infrastructure.WeiXin.Common.Ticket;

    public class Order
    {
        protected IOrderOperate _orderOperate;

        protected IPaymentOperate _paymentOperate;

        private EventHandlerList _events;

        public OrderRequestEntity OrderRequest;

        public OrderEntity OrderObj;

        //public IList<DateTicketEntity> DateTicketList;

        public IList<DailyProductEntity> dailyProducts;

        internal static readonly object EventPreValidate = new Object();
        internal static readonly object EventPreCreateOrder = new Object();
        internal static readonly object EventCreateOrderComplete = new Object();
        internal static readonly object EvnetRefundPayComplete = new object();

        public Order(OrderRequestEntity orderRequest)
        {
            this.OrderRequest = orderRequest;
        }

        public Order(OrderEntity order)
        {
            this.OrderObj = order;
        }

        #region Events
        /// <summary>
        /// Gets the events.
        /// </summary>
        protected EventHandlerList Events
        {
            get
            {
                return this._events ?? (this._events = new EventHandlerList());
            }
        }


        public event EventHandler PreValidate
        {
            add
            {
                this.Events.AddHandler(EventPreValidate, value);
            }

            remove
            {
                this.Events.RemoveHandler(EventPreValidate, value);
            }
        }

        public event EventHandler PreCreateOrder
        {
            add
            {
                this.Events.AddHandler(EventPreCreateOrder, value);
            }

            remove
            {
                this.Events.RemoveHandler(EventPreCreateOrder, value);
            }
        }

        public event EventHandler CreateOrderComplete
        {
            add
            {
                this.Events.AddHandler(EventCreateOrderComplete, value);
            }

            remove
            {
                this.Events.RemoveHandler(EventCreateOrderComplete, value);
            }
        }

        public event EventHandler RefundPayComplete
        {
            add
            {
                this.Events.AddHandler(EvnetRefundPayComplete, value);
            }

            remove
            {
                this.Events.RemoveHandler(EvnetRefundPayComplete, value);
            }
        }

        protected virtual void OnPreValidate(EventArgs e)
        {
            var handler = (EventHandler)this.Events[EventPreValidate];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPreCreateOrder(EventArgs e)
        {
            var handler = (EventHandler)this.Events[EventPreCreateOrder];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCreateOrderComplete(EventArgs e)
        {
            var handler = (EventHandler)this.Events[EventCreateOrderComplete];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public class RefundEventArgs : EventArgs
        {
            public OrderDetailEntity orderDetail { get; set; }

            public RefundOrderEntity refundOrder { get; set; }

            public ICollection<TicketEntity> tickets { get; set; }
        }

        protected virtual void OnRefundPayComplete(EventArgs e)
        {
            var handler = (EventHandler)this.Events[EvnetRefundPayComplete];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region 生成订单相关方法

        /// <summary>
        /// 生成订单主方法
        /// </summary>
        public void CreateOrderMain()
        {
            try
            {
                this.OnPreValidate(EventArgs.Empty);
                this.OnPreCreateOrder(EventArgs.Empty);
                this.ConbinedOrderComponent();
                this.OnCreateOrderComplete(EventArgs.Empty);
            }
            catch (OrderOperateFailException orderException)
            {
                this.ProcessOrderOperationException(orderException);
            }
            catch (OrderPaymentFailException orderPaymentException)
            {
                this.ProcessOrderPaymentException(orderPaymentException);
            }
            catch (TimeoutException ex)
            {
                var exLog = new ExceptionLogEntity()
                {
                    ExceptionLogId = Guid.NewGuid(),
                    Module = this.OrderObj != null ? "生成订单请求外部接口" + "------" + this.OrderObj.OrderCode : "生成订单请求外部接口",
                    CreateTime = DateTime.Now,
                    ExceptionType = ex.GetType().FullName,
                    ExceptionMessage = ex.Message,
                    TrackMessage = ex.StackTrace,
                    HasExceptionProcessing = false,
                    NeedProcess = false,
                    ProcessFunction = string.Empty
                };

                exLog.Add();

                throw;
            }
            catch (Exception ex)
            {
                var exLog = new ExceptionLogEntity()
                {
                    ExceptionLogId = Guid.NewGuid(),
                    Module = this.OrderObj != null ? "生成订单" + "------" + this.OrderObj.OrderCode : "生成订单",
                    CreateTime = DateTime.Now,
                    ExceptionType = ex.GetType().FullName,
                    ExceptionMessage = ex.Message,
                    TrackMessage = ex.StackTrace,
                    HasExceptionProcessing = false,
                    NeedProcess = false,
                    ProcessFunction = string.Empty
                };

                exLog.Add();
                throw;
            }
        }

        /// <summary>
        /// 组合订单各项
        /// </summary>
        protected void ConbinedOrderComponent()
        {
            this.CreateOrder();

            if (this.OrderObj != null)
            {
                var orderDetail = this.CreateOrderDetail();

                if (orderDetail != null)
                {
                    this.OrderObj.OrderDetails = orderDetail;
                }
                else
                {
                    throw new OrderOperateFailException("创建订单明细失败", OrderOperationStep.OrderCreate);
                }

                var tickets = this.CreateTicket();

                if (tickets != null && tickets.Any())
                {
                    this.OrderObj.Tickets = tickets;
                }
                else
                {
                    throw new OrderOperateFailException("创建售出票失败", OrderOperationStep.OrderCreate);
                }
            }
            else
            {
                throw new OrderOperateFailException("创建订单主体失败", OrderOperationStep.OrderCreate);
            }

            try
            {
                this.OrderObj.AddOrder();
            }
            catch (Exception)
            {
                throw new OrderOperateFailException("订单保存失败", OrderOperationStep.OrderCreate, "SAVEDATA_FAIL");
            }
        }

        /// <summary>
        /// 创建用户申请的门票
        /// </summary>
        /// <returns></returns>
        protected IList<TicketEntity> CreateTicket()
        {
            var tickets = new List<TicketEntity>();

            if (this.dailyProducts.Any())
            {
                var product =
                    this.dailyProducts.FirstOrDefault(
                        item => item.ProductCategoryId.Equals(Guid.Parse(this.OrderRequest.TicketCategory)));

                for (var i = 0; i < this.OrderRequest.Count; i++)
                {
                    if (product != null)
                    {
                        tickets.Add(new TicketEntity()
                        {
                            TicketId = new Random(unchecked((int)DateTime.Now.Ticks + i)).Next(1000000, 9999999),
                            OrderId = this.OrderObj.OrderId,
                            RefundOrderId = default(Guid?),
                            RefundOrderDetailId = default(Guid?),
                            OrderDetailId = this.OrderObj.OrderDetails.First().OrderDetailId,
                            TicketCategoryId = product.ProductCategoryId,
                            TicketCode = product.ProductCode,
                            TicketProductId = product.ProductId,
                            Price = product.ProductPrice,
                            TicketStatus = OrderStatus.TicketStatus_Init,
                            ECode = string.Empty,
                            CreateTime = DateTime.Now,
                            LatestModifyTime = DateTime.Now,
                            TicketStartTime = DateTime.Now,
                            TicketEndTime = DateTime.Now.AddYears(1)
                        });
                    }
                    else
                    {
                        tickets.Clear();
                        break;
                    }
                }
            }

            return tickets;
        }

        /// <summary>
        /// 创建用户订单
        /// </summary>
        protected void CreateOrder()
        {
            this.OrderObj = new OrderEntity()
                         {
                             OrderId = Guid.NewGuid(),
                             OrderCode = this.CreateOrderCode(),
                             WXOrderCode = string.Empty,
                             CreateTime = DateTime.Now,
                             OpenId = this.OrderRequest.OpenId,
                             PreUseTime = this.OrderRequest.PreUseTime,
                             ContactPersonName = this.OrderRequest.ContactPersonName,
                             MobilePhoneNumber = this.OrderRequest.MobilePhoneNumber,
                             IdentityCardNumber = this.OrderRequest.IdentityCardNumber,
                             OrderStatus = OrderStatus.OrderStatus_Init,
                             HasCoupon = !string.IsNullOrEmpty(this.OrderRequest.CouponId),
                             CouponId = string.IsNullOrEmpty(this.OrderRequest.CouponId) ? default(Guid?) : Guid.Parse(this.OrderRequest.CouponId),
                             OrderDetails = new List<OrderDetailEntity>(),
                             Tickets = new List<TicketEntity>()
                         };
        }

        /// <summary>
        /// 创建用户订单详情
        /// </summary>
        /// <returns></returns>
        protected IList<OrderDetailEntity> CreateOrderDetail()
        {
            var product =
                this.dailyProducts.FirstOrDefault(
                    item => item.ProductCategoryId.Equals(Guid.Parse(this.OrderRequest.TicketCategory)));

            if (product != null)
            {
                return new List<OrderDetailEntity>()
                           {
                               new OrderDetailEntity()
                                   {
                                       OrderDetailId = Guid.NewGuid(),
                                       OrderId = this.OrderObj.OrderId,
                                       OrderDetailCategoryId = Guid.Parse(OrderStatus.OrderDetailCategory_Create),
                                       TicketCategoryId = product.ProductCategoryId,
                                       Count = this.OrderRequest.Count,
                                       SingleTicketPrice = product.ProductPrice,
                                       IsDiscount = false,
                                       DiscountCategoryId = default(Guid?),
                                       TotalPrice = product.ProductPrice * this.OrderRequest.Count
                                   }
                           };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 创建订单编码
        /// </summary>
        /// <returns></returns>
        protected string CreateOrderCode()
        {
            return "C" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + this.GetRandom();
        }

        private static Random myRan = new Random();

        private string GetRandom()
        {
            return myRan.Next(1000, 9998).ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region 退票请求处理

        /// <summary>
        /// 退票请求处理
        /// </summary>
        /// <param name="tickets">此参数必须通过ECode从数据库查询得到</param>
        public void ProcessRefundRequestMain(IList<TicketEntity> tickets)
        {
            try
            {
                if (this.IsRefundRequestCorrect(tickets))
                {
                    this.ProcessRefund(tickets);
                }
            }
            catch (OrderOperateFailException orderException)
            {
                this.ProcessOrderOperationException(orderException);
            }
            catch (Exception ex)
            {
                var exLog = new ExceptionLogEntity()
                {
                    ExceptionLogId = Guid.NewGuid(),
                    Module = "退票请求处理" + "------" + this.OrderObj.OrderCode,
                    CreateTime = DateTime.Now,
                    ExceptionType = ex.GetType().FullName,
                    ExceptionMessage = ex.Message,
                    TrackMessage = ex.StackTrace,
                    HasExceptionProcessing = false,
                    NeedProcess = false,
                    ProcessFunction = string.Empty
                };

                exLog.Add();
            }
        }

        protected virtual void ProcessRefund(ICollection<TicketEntity> tickets)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 判断申请退票操作的票的状态是否正确
        /// </summary>
        /// <param name="tickets"></param>
        /// <returns></returns>
        private bool IsRefundRequestCorrect(IEnumerable<TicketEntity> tickets)
        {
            var canTicketRefund = false;

            foreach (var ticket in tickets)
            {
                canTicketRefund = ticket.TicketStatus.Equals(OrderStatus.TicketStatus_WaitUse);

                if (!canTicketRefund)
                {
                    break;
                }
            }

            return canTicketRefund;
        }

        #endregion

        #region 退款处理

        /// <summary>
        /// 处理退款
        /// </summary>
        /// <param name="tickets">所有tickets必须是同一个订单的</param>
        public void ProcessRefundPayment(ICollection<TicketEntity> tickets)
        {
            if (this.OrderObj != null)
            {
                var orderDetail = new OrderDetailEntity()
                                      {
                                          OrderDetailId = Guid.NewGuid(),
                                          OrderDetailCategoryId = Guid.Parse(OrderStatus.OrderDetailCategory_Refund),
                                          OrderId = this.OrderObj.OrderId,
                                          TicketCategoryId = tickets.First().TicketCategoryId,
                                          Count = tickets.Count,
                                          SingleTicketPrice = tickets.First().Price,
                                          IsDiscount = false,
                                          DiscountCategoryId = default(Guid?)
                                      };
                orderDetail.TotalPrice = orderDetail.TotalFee();

                var refundOrder = new RefundOrderEntity()
                                      {
                                          RefundOrderId = Guid.NewGuid(),
                                          OrderId = this.OrderObj.OrderId,
                                          RefundOrderCode = this.CreateRefundOrderCode(),
                                          WXRefundOrderCode = string.Empty,
                                          CreateTime = DateTime.Now,
                                          RefundStatus = OrderStatus.RefundOrderStatus_Init,
                                          LatestModifyTime = DateTime.Now,
                                          OperatorName = "后台管理员"
                                      };

                foreach (var refundTicket in tickets)
                {
                    refundTicket.RefundOrderId = refundOrder.RefundOrderId;
                    refundTicket.RefundOrderDetailId = orderDetail.OrderDetailId;
                    refundTicket.TicketStatus = OrderStatus.TicketStatus_Refund_RefundPayProcessing;
                    refundTicket.LatestModifyTime = DateTime.Now;
                }

                using (var scope = new TransactionScope())
                {
                    refundOrder.Add();
                    orderDetail.Add();
                    TicketEntity.ModifyTickets(tickets);

                    scope.Complete();
                }


                var eventArgs = new RefundEventArgs
                                    {
                                        orderDetail = orderDetail,
                                        refundOrder = refundOrder,
                                        tickets = tickets
                                    };

                try
                {
                    this.OnRefundPayComplete(eventArgs);
                }
                catch (OrderPaymentFailException orderException)
                {
                    orderException.param = new Dictionary<string, object>
                                               {
                                                   { "refundOrder", refundOrder },
                                                   { "orderDetail", orderDetail },
                                                   { "tickets", tickets }
                                               };
                    this.ProcessOrderPaymentException(orderException);
                    //throw;
                }
                catch (WxPayException ex)
                {
                    var orderException = new OrderPaymentFailException("无法在制定的路径中找到秘钥", OrderPaymentStep.ApplyRefund, "CRYPTOGRAPHY_ERROR");
                    orderException.param = new Dictionary<string, object>
                                               {
                                                   { "refundOrder", refundOrder },
                                                   { "orderDetail", orderDetail },
                                                   { "tickets", tickets }
                                               };

                    this.ProcessOrderPaymentException(orderException);
                }
                catch (Exception ex)
                {
                    var exLog = new ExceptionLogEntity()
                    {
                        ExceptionLogId = Guid.NewGuid(),
                        Module = "微信退款" + "------" + this.OrderObj.OrderCode,
                        CreateTime = DateTime.Now,
                        ExceptionType = ex.GetType().FullName,
                        ExceptionMessage = ex.Message,
                        TrackMessage = ex.StackTrace,
                        HasExceptionProcessing = false,
                        NeedProcess = false,
                        ProcessFunction = string.Empty
                    };

                    exLog.Add();

                    throw;
                }

            }
        }

        protected static void WriteLog(string content)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "log";
            if (!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";//用日期对日志文件命名

            //创建或打开日志文件，向日志文件末尾追加记录
            StreamWriter mySw = File.AppendText(filename);

            mySw.WriteLine(time + "   |" + content + Environment.NewLine);

            //关闭日志文件
            mySw.Close();
        }

        protected string CreateRefundOrderCode()
        {
            return "T" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + this.GetRandom();
        }

        #endregion

        #region 未支付订单

        public static IList<OrderEntity> GetOverTimeOrder(int overtimeSeconds)
        {
            return OrderEntity.GetOrders(
                    item =>
                        item.OrderStatus.Equals(OrderStatus.OrderStatus_WaitPay)
                        && item.CreateTime.AddSeconds(overtimeSeconds).CompareTo(DateTime.Now) < 0);
        }

        #endregion

        #region 异常处理流程

        public void ProcessOrderOperationException(OrderOperateFailException orderExcepion)
        {
            // todo: 第一步，记录异常日志；第二步，异常处理；第三步，记录处理方式
            var needProcess = false;
            var processMethod = string.Empty;
            var isProcessSuccess = true;

            switch (orderExcepion.OperationStep)
            {
                case OrderOperationStep.GetDailyTicket:
                    break;
                case OrderOperationStep.OrderOccupy:
                    this.OrderObj.DeleteOrder();

                    needProcess = true;
                    processMethod = "回滚Order表中的数据";
                    break;
                case OrderOperationStep.OrderCreate:

                    break;
                case OrderOperationStep.OrderChange:
                    break;
                case OrderOperationStep.GetOrderStatus:
                    break;
                default:
                    break;
            }

            var exLog = new ExceptionLogEntity()
            {
                ExceptionLogId = Guid.NewGuid(),
                Module = this.OrderObj != null ? Enum.GetName(typeof(OrderOperationStep), orderExcepion.OperationStep) + "------" + this.OrderObj.OrderCode
                                                : Enum.GetName(typeof(OrderOperationStep), orderExcepion.OperationStep),
                CreateTime = DateTime.Now,
                ExceptionType = orderExcepion.GetType().FullName,
                ExceptionMessage = orderExcepion.Message,
                TrackMessage = orderExcepion.StackTrace,
                HasExceptionProcessing = true,
                NeedProcess = needProcess,
                ProcessFunction = processMethod
            };

            exLog.Add();
        }

        public void ProcessOrderPaymentException(OrderPaymentFailException orderException)
        {
            var needProcess = false;
            var processMethod = string.Empty;
            var isProcessSuccess = true;

            switch (orderException.PaymentStep)
            {
                case OrderPaymentStep.UnifiedOrder:
                    var result = this._orderOperate.OrderRelease();

                    if (result.IsTrue)
                    {
                        this.OrderObj.DeleteOrder();

                        needProcess = true;
                        processMethod = "回滚Order表中的数据";
                    }
                    else
                    {
                        needProcess = true;
                        processMethod = "统一下单失败，释放订单失败";
                        isProcessSuccess = false;
                    }
                    break;
                case OrderPaymentStep.ApplyRefund:
                    if (orderException.param.ContainsKey("refundOrder")
                        && orderException.param.ContainsKey("orderDetail")
                        && orderException.param.ContainsKey("tickets"))
                    {
                        var refundOrder = orderException.param["refundOrder"] as RefundOrderEntity;
                        var orderDetail = orderException.param["orderDetail"] as OrderDetailEntity;
                        var tickets = orderException.param["tickets"] as ICollection<TicketEntity>;

                        refundOrder.Delete();
                        orderDetail.Delete();
                        foreach (var ticket in tickets)
                        {
                            ticket.TicketStatus = OrderStatus.TicketStatus_Refund_Audit;
                            ticket.LatestModifyTime = DateTime.Now;
                            ticket.RefundOrderId = default(Guid?);
                            ticket.RefundOrderDetailId = default(Guid);
                        }

                        TicketEntity.ModifyTickets(tickets);

                        needProcess = true;
                        processMethod = "清除退款订单及其订单详情，回滚门票状态";
                    }

                    break;
                case OrderPaymentStep.PaymentResultNotify:
                    break;
                case OrderPaymentStep.RefundQuery:
                    break;
                default:
                    break;
            }

            var exLog = new ExceptionLogEntity()
            {
                ExceptionLogId = Guid.NewGuid(),
                Module = this.OrderObj != null ? Enum.GetName(typeof(OrderOperationStep), orderException.PaymentStep) + "------" + this.OrderObj.OrderCode
                                                : Enum.GetName(typeof(OrderOperationStep), orderException.PaymentStep),
                CreateTime = DateTime.Now,
                ExceptionType = orderException.GetType().FullName,
                ExceptionMessage = orderException.Message,
                TrackMessage = orderException.StackTrace,
                HasExceptionProcessing = true,
                NeedProcess = needProcess,
                ProcessFunction = processMethod
            };

            exLog.Add();
        }

        #endregion
    }
}
