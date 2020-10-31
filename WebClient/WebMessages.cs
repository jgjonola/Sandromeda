

namespace WebClient
{
    public class WebMessages
    {
        public string m_id;
        public string m_message;
        public bool processed;
        public string m_db_id;

        public WebMessages(string id, string message, string db_id)
        {
            m_db_id = db_id;
            m_id = id;
            m_message = message;
            processed=false;
        }

    }
}
