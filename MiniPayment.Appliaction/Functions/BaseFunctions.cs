using System.Text;

namespace MiniPayment.Appliaction.Functions;

public class BaseFunctions
{
   
    public static string GenerateCode(int length = 6)
    {
        StringBuilder value = new(); //Boş bir değer tanımlıyoruz.
        Random rnd = new(); // Burada Rastgele değeri tanımlıyouz.
        for (int c = 0; c < length; c++) //25 haneli rakam-harf üretmek için döngü yaptık.
        {
            int ck = rnd.Next(0, 2); // 0 veya 1
            if (ck == 0) // Rastgele üretilen sayı 0 ise sayı üret.
            {
                int num = rnd.Next(1, 10);
                value.Append(num);
            }
            else // Değilse harf üret (65 ile 91 arası ascii kodlar olduğu için rakam değerleri girdik.)
            {
                int x = rnd.Next(65, 91);
                char chr = Convert.ToChar(x); //ascii kod olarak üretilen sayıyı harfe çevirdik.
                value.Append(chr); //Değere atadık.
            }
        }
        return value.ToString();
    }

}
