using System;

namespace WeiXinPF.Model
{
    /// <summary>
    /// ������
    /// </summary>
    [Serializable]
    public partial class article_attach
    {
        public article_attach()
        { }
        #region Model
        private int _id;
        private int _article_id = 0;
        private string _file_name = "";
        private string _file_path = "";
        private int _file_size = 0;
        private string _file_ext = "";
        private int _down_num = 0;
        private int _point = 0;
        private DateTime _add_time = DateTime.Now;
        /// <summary>
        /// ����ID
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// ����ID
        /// </summary>
        public int article_id
        {
            set { _article_id = value; }
            get { return _article_id; }
        }
        /// <summary>
        /// �ļ���
        /// </summary>
        public string file_name
        {
            set { _file_name = value; }
            get { return _file_name; }
        }
        /// <summary>
        /// �ļ�·��
        /// </summary>
        public string file_path
        {
            set { _file_path = value; }
            get { return _file_path; }
        }
        /// <summary>
        /// �ļ���С(�ֽ�)
        /// </summary>
        public int file_size
        {
            set { _file_size = value; }
            get { return _file_size; }
        }
        /// <summary>
        /// �ļ���չ��
        /// </summary>
        public string file_ext
        {
            set { _file_ext = value; }
            get { return _file_ext; }
        }
        /// <summary>
        /// ���ش���
        /// </summary>
        public int down_num
        {
            set { _down_num = value; }
            get { return _down_num; }
        }
        /// <summary>
        /// ����(�����͸�����)
        /// </summary>
        public int point
        {
            set { _point = value; }
            get { return _point; }
        }
        /// <summary>
        /// �ϴ�ʱ��
        /// </summary>
        public DateTime add_time
        {
            set { _add_time = value; }
            get { return _add_time; }
        }
        #endregion Model

    }
}