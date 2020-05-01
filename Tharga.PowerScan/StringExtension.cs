namespace Tharga.PowerScan
{
    public static class StringExtension
    {
        public static string Padd(this string item, int length)
        {
            if (item == null) return null;

            if (item.Length > length)
            {
                return item.Substring(0, length);
            }

            if (item.Length < length)
            {
                return $"{item}{new string(' ', length - item.Length)}";
            }

            return item;
        }
    }
}