using DynamicDNS.Api.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DynamicDNS.Core {

    /// <summary>
    /// 域名辅助类
    /// </summary>
    public class DNSHelper  {

        private static readonly KeyValuePair<string, string>[] urls = new KeyValuePair<string, string>[] {
            new KeyValuePair<string, string>("http://ip.qq.com/", @"\d+\.\d+\.\d+\.\d+" ),
            new KeyValuePair<string, string> ("http://gz.bendibao.com/ip/ip.asp", @"\d+\.\d+\.\d+\.\d+" ),
            new KeyValuePair<string, string> ("http://www.meibu.com/ip.asp", @"\d+\.\d+\.\d+\.\d+" ),
            new KeyValuePair<string, string> ("http://sgld.org/ip", @"\d+\.\d+\.\d+\.\d+" ),
            new KeyValuePair<string, string> ("http://uuuuer.sinaapp.com/ip", ""),
            new KeyValuePair<string, string> ("http://www.wuyuanhe.com/ip", "" )
        };

        /// <summary>
        /// 获取本机外网IP，从五个源获取
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP() {
            var ip = string.Empty;
            var i = 0;
            var isLookup = false;

            while (string.IsNullOrWhiteSpace(ip) && i < urls.Length) {

                if (isLookup == false) {
                    isLookup = true;

                    try {
                        var item = urls[i];
                        Logger.Write("从{0}获取IP地址", item.Key);
                        var result = HttpHelper.Get(item.Key);

                        if (!string.IsNullOrWhiteSpace(result)) {
                            if (!string.IsNullOrWhiteSpace(item.Value))
                                ip = Regex.Match(result, item.Value).Result("$0");
                            else
                                ip = result.Trim();

                            Logger.Write("得到IP地址：{0}", ip);
                        }
                    }
                    catch (Exception ex) {
                        Logger.Write("获取IP失败：{0}", ex.Message);
                    }

                    i++;
                    isLookup = false;
                }
            }

            return ip;
        }
    }
}
