﻿using Xamarin.Forms;

namespace Malla.CardIO
{
    /// <summary>
    /// Class holding an Instance to the dependeny service of <c>ICardIO</c>
    /// </summary>
    public sealed class CardIO
    {
        private static ICardIO _instance;
        /// <summary>
        /// Gets instance of the dependency service of <c>ICardIO</c>
        /// </summary>
        public static ICardIO Instance
        {
            get
            {
                return _instance ?? (_instance = DependencyService.Get<ICardIO>());
            }
        }
    }
}
