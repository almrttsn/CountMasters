using MatchinghamGames.DealerModule;
using MatchinghamGames.DealerModule.Models;
using MatchinghamGames.SherlockModule;
using MatchinghamGames.SherlockModule.Services.Admost.Runtime;
using System.Collections;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers
{
    public class IAPManager : CustomBehaviour
    {
        public override void Initialize(GameManager gameManager)
        {
            base.Initialize(gameManager);

            Dealer.Service.Purchased += Purchased;
            Dealer.Service.RestoreSuccess += RestoreSuccess;
        }

        private void OnDestroy()
        {
            Dealer.Service.Purchased -= Purchased;
            Dealer.Service.RestoreSuccess -= RestoreSuccess;
        }

        private void RestoreSuccess()
        {
            Debug.Log("Restore Success");

            //if (Dealer.Instance.IsPurchased(Dealer.Instance.GetProduct("com.matchingham.islandsort.noads"))
            //    || Dealer.Instance.IsPurchased(Dealer.Instance.GetProduct("com.matchingham.islandsort.noadsoffer")))
            //{
            //    OnPreviouslyPurchasedProductCallback("NoAds");
            //}
        }

        private void Purchased(PurchaseInfo info, bool notifyUser)
        {
            Debug.Log("Purchase Success");

            string id = info.purchasedProduct.Id;
            //if (id == "com.matchingham.islandsort.noads")
            //{
            //    var product = Dealer.Instance.GetProduct(id);
            //    OnProductPurchasedCallBackWithAnalytics(product.Metadata.Title, product.Metadata.Price, product.Metadata.IsoCurrencyCode, "iap_noads");
            //}
            //else if (id == "com.matchingham.islandsort.noadsoffer")
            //{
            //    var product = Dealer.Instance.GetProduct(id);
            //    OnProductPurchasedCallBackWithAnalytics(product.Metadata.Title, product.Metadata.Price, product.Metadata.IsoCurrencyCode, "iap_noadsoffer");
            //}
        }

        private void OnProductPurchasedCallBackWithAnalytics(string purchasedProduct, float price, string currencyCode, string receipt)
        {
            //switch (purchasedProduct)
            //{
            //    case "NoAds":                    
            //        GameManager.AnalyticsManager.SendAdjustIAPEvents("iap_noads", price, receipt, currencyCode);
            //        break;

            //    case "DiscountNoAdsOffer":                    
            //        GameManager.AnalyticsManager.SendAdjustIAPEvents("iap_noadsoffer", price, receipt, currencyCode);
            //        break;
            //}
        }

        private void OnPreviouslyPurchasedProductCallback(string purchasedProduct)
        {
            //switch (purchasedProduct)
            //{
            //    case "NoAds":
            //        OnNoAdsPurchased?.Invoke();
            //        break;

            //    case "DiscountNoAdsOffer":
            //        OnNoAdsPurchased?.Invoke();
            //        break;
            //}
        }

        public void PurchaseProduct(string productId)
        {
            Dealer.Service.RequestPurchase(productId);
        }
        public string GetItemPrice(string productId)
        {
            var iapProduct = Dealer.Service.GetProduct(productId);
            return iapProduct.Metadata.Price.ToString();
        }
        public bool IsProductPurchased(string productId)
        {
            var iapProduct = Dealer.Service.GetProduct(productId);
            return Dealer.Instance.IsPurchased(iapProduct);            
        }
        public void RestorePurchases()
        {
            Dealer.Service.RequestRestore();
        }
    }
}