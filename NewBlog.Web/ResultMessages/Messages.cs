namespace NewBlog.Web.ResultMessages
{
    public static class Messages
    {
        public static class ArticleMessage
        {
            public static string Add(string articleTitle)
            {
                return $"'{articleTitle}' Created Successfully";
            }

            public static string Update(string articleTitle)
            {
                return $"'{articleTitle}' Updated Successfully";
            }

            public static string Delete(string articleTitle)
            {
                return $"'{articleTitle}' Deleted Successfully";
            }

            public static string UndoDelete(string articleTitle)
            {
                return $"'{articleTitle}' Restored Successfully";
            }
        }

        public static class CategoryMessage
        {
            public static string Add(string categoryName)
            {
                return $"'{categoryName}' Created Successfully";
            }

            public static string Update(string categoryName)
            {
                return $"'{categoryName}' Updated Successfully";
            }

            public static string Delete(string categoryName)
            {
                return $"'{categoryName}' Deleted Successfully";
            }

            public static string UndoDelete(string categoryName)
            {
                return $"'{categoryName}' Restored Successfully";
            }
        }

        public static class UserMessage
        {
            public static string Add(string userName)
            {
                return $"'{userName}' Created Successfully";
            }

            public static string Update(string userName)
            {
                return $"'{userName}' Updated Successfully";
            }

            public static string Delete(string userName)
            {
                return $"'{userName}' Deleted Successfully";
            }
        }
    }
}
