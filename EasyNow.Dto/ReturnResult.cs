namespace EasyNow.Dto
{
    /// <summary>
    /// ���ؽ��
    /// </summary>
    /// <typeparam name="TCode"></typeparam>
    public class ReturnResult<TCode>
    {
        /// <summary>
        /// ���
        /// </summary>
        public TCode Code { get; set; }
        
        /// <summary>
        /// ��Ϣ
        /// </summary>
        public string Msg { get; set; }
    }

    /// <summary>
    /// �����ݷ��ؽ��
    /// </summary>
    /// <typeparam name="TCode"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class ReturnResult<TCode, TData> : ReturnResult<TCode>
    {
        /// <summary>
        /// ����
        /// </summary>
        public TData Data { get; set; }
    }
}