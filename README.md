# XorPay_Net [![license](https://img.shields.io/github/license/HeavenJoe/xorpay_net.svg?v=2019)](https://www.mit-license.org/)
个人支付（微信、支付宝）解决方案 [XorPay](https://xorpay.com/?r=quickpay) 的.Net版本SDK与Demo。

[在线演示Demo](https://xorpay.jinliniuan.com/)

XorPay简介：
----------------
个人可用的支付宝/微信支付接口，支持当面付/NATIVE/JSAPI/收银台/小程序/WAP/H5 等支付方式，资金由支付宝/微信官方D1结算自动下发个人银行卡。

[查看官网](https://xorpay.com/?r=quickpay)

配置说明：
----------------
项目初次使用，记得先配置XorPay.SDK文件夹下PayConfig.cs文件中的aid与app_secret，最好在服务器端测试。

项目打开提示："这台计算机上缺少此项目引用的 NuGet 程序包。使用“NuGet 程序包还原”可下载这些程序包。有关更多信息，请参见..."，打开VS菜单：工具 > NuGet包管理器 > 管理解决方案的NuGet程序包，点击最上方的还原即可联机下载恢复，或者在解决方案资源管理器选择项目解决方案右键，选择还原NuGet包。
