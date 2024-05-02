using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace CacheExample.Controllers
{
    public class EmployeeController : Controller
    {
        IMemoryCache _memoryCache;

        public EmployeeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get – Set Fonksiyonları
        //İsimlerinden de anlaşılacağı üzere memory’e data cachelemek ve cachelenen datayı okumak için Get ve Set metotları kullanılmaktadır.key – value diziliminde olan bu metotlardan Set metodu datayı cachelemek, Get metodu ise cache’de ki datayı okumak/elde etmek için kullanılır.
        public IActionResult SetCache()
        {
            _memoryCache.Set("employeeName", "Muiddin Impatrino");
            return View();
        }

        public IActionResult GetCache()
        {
            string name = _memoryCache.Get<string>("employeeName");
            return View();
        }

        //Remove Fonksiyonu
        //Cachelenmiş datayı silmek için kullanılır.
        public void RemoveCache()
        {
            _memoryCache.Remove("employeeName");
        }


        //TryGetValue Fonksiyonu
        //Cache’de belirtilen key değerine uygun veriyi sorgular.Veri yoksa ‘false’ eğer varsa ‘true’ döndürerek ‘out’ olan ikinci parametresinde de cacheden datayı döndürür.
        public IActionResult TryGetValueCache()
        {
            if (_memoryCache.TryGetValue<string>("employeeName", out string data))
            {
                //data burada elde edilmiştir
            }
            return View();
        }


        //Belirtilen key değerinde data var mı kontrol eder, yoksa oluşturur.
        public IActionResult GetOrCreate()
        {
            string name = _memoryCache.GetOrCreate<string>("employeeName", entry =>
            {
                entry.SetValue("Muiddin Impatrino");
                Console.WriteLine(DateTime.Now);
                return entry.Value.ToString();
            });

            return View();
        }

        //Absolute&Sliding Expiration
        //Cache’de tutulacak datanın yaşam süresini belirlememizi sağlayan özelliklerdir.Absolute, datanın cache’de tutulma süresini belirlerken; Sliding, belirtilen süre zarfında cache’den data talep edilirse eğer bir o kadar daha tutulma süresini uzatacak aksi taktirde datayı silecektir.Her ikiside aynı anda kullanıldığı zaman sliding, absolute’te belirtilen süre dolana kadar periyodik işlevine devam edecektir.

        public void AbsoluteSliding()
        {
            DateTime date = _memoryCache.GetOrCreate<DateTime>("date", entry =>
            {
                entry.AbsoluteExpiration = DateTime.Now.AddSeconds(30);//Cache'de ki datanın ömrü 10 saniye olarak belirlenmiştir.
                entry.SlidingExpiration = TimeSpan.FromSeconds(5);//Cache'de ki datanın ömrü 2 saniye olarak belirlenmiştir.
                                                                  //2 saniye içerisinde bir istek yapılırsa kalış süresi 2 saniye daha uzayacaktır.
                                                                  //Absolute değeri belirtildiğinden dolayı bu süreç totalde 2 saniye boyunca sürecektir.
                DateTime value = DateTime.Now;
                Console.WriteLine($"*** Set Cache : {value}");
                return value;
            });

            Console.WriteLine($"Get Cache : {date}");
        }



        public IActionResult AbsoluteSlidingTime()
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(30);
            options.SlidingExpiration = TimeSpan.FromSeconds(5);
            _memoryCache.Set("date", DateTime.Now, options);

            return RedirectToAction(nameof(GetCache));
        }
        public IActionResult AbsoluteSlidingTryGetValue()   
        {
            if (_memoryCache.TryGetValue<DateTime>("date", out DateTime date))
            {
                Console.WriteLine($"Get Cache : {date}");
            }
            return View();
        }


        //Cache Priority

        //Yayın süreci boyunca cache’e depolanan veriler memory’i haddinden fazla şişirebilir ve yeni veriler için sistem tarafından var olan veriler silinmek istenebilir. İşte böyle bir durumda cache’den silinecek olan verilerin önceliklerini ve hangilerinin kalıcı olacağını Priority değeri aracılığıyla belirlemekteyiz

        //Priority değeri; Low, Normal, High ve NeverRemove olmak üzere dört değer almaktadır. Bu değerleri sırasıyla açıklarsak eğer;
        //Low : Önem derecesi en düşük olan datadır.İhtiyaç doğrultusunda ilk silinecek datadır.
        //Normal : Önem derecesi Low’dan sonra gelen datadır.
        //High : Önemli veridir. Çok zaruri olduğu taktirde cache’den silinecektir.
        //NeverRemove : Kesinlikle silinmemesi gereken datadır. Sınıra gelinen bir memory’de Priority değeri NeverRemove olan datalarla sınır aşılırsa exception fırlatılır.
        public void CachePriority()
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.Priority = CacheItemPriority.High;
            _memoryCache.Set("date", DateTime.Now, options);
        }

    }
}
