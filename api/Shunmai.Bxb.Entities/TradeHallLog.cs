using System;

namespace Shunmai.Bxb.Entities
{
    ///<summary>
    /// ������־
    ///</summary>
    public class TradeHallLog
    {
        ///<summary>
        /// ����
        ///</summary>
        public long LogId { get; set; }

        ///<summary>
        /// ���״���ID
        ///</summary>
        public int TradeHallId { get; set; }

        ///<summary>
        /// ������
        ///</summary>
        public int OperateId { get; set; }

        ///<summary>
        /// ����������
        ///</summary>
        public string OperateName { get; set; }

        ///<summary>
        /// ��������
        ///</summary>
        public string OperateLog { get; set; }

        ///<summary>
        /// ����ʱ��
        ///</summary>
        public DateTime CreateTime { get; set; }
    }
}

