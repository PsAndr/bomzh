namespace Engine
{
    public static class FindInArray
    {
        public static int Find<T>(T elem, ref T[] values)
        {
            int i = 0;
            foreach (var v in values)
            {
                if (v.Equals(elem))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
    }
}
