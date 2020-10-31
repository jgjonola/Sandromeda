

namespace WebClient
{
    public class WebMessages
    {
        public string m_id;
        public string m_message;
        public bool processed;
        public string m_dbid;

        public WebMessages(string id, string message, string dbid)
        {
            m_id = id;
            m_message = message;
            m_dbid = dbid;
            processed=false;
        }

    }
}
