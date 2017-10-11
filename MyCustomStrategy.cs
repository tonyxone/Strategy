#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Indicator;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Strategy;
#endregion

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    /// <summary>
    /// Enter the description of your strategy here
    /// </summary>
    [Description("Enter the description of your strategy here")]
    public class MyCustomStrategy : Strategy
    {
        #region Variables
        // Wizard generated variables
        private int myInput0 = 1; // Default setting for MyInput0
        // User defined variables (add any user defined variables below)
        #endregion

		private bool prevSMAAllAbove;
		private bool prevSMAAllBelow;
		private bool bought = false;
		private double boughtPoint = 0;
		private double breakEvenPoint = 0;
		private bool setBreakEvenPoint = false;
		
        /// <summary>
        /// This method is used to configure the strategy and is called once before any strategy method is called.
        /// </summary>
        protected override void Initialize()
        {
			Add(SMA(5));
            Add(SMA(20));
			Add(SMA(30));
			Add(SMA(90));
            CalculateOnBarClose = true;
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
			
			bool smaAllAbove = this.smaAllAbove();
			bool smaAllBelow = this.smaAllBelow();
			
			double pl = 0;
			
			//*****************************************************************************
			//buying
			
			if(bought != true){
				if(smaAllBelow == false && prevSMAAllBelow == true){
					EnterLong(1, "long1");
					boughtPoint = Close[0];
					bought = true;
				}
			}
			
			if(bought == true){
				
				if(CrossBelow(Close,SMA(30),1)){
					ExitLong();
					bought = false;
				}
				
				
			}
			
		
			//********************************************************************************6
			//selling
			
			//if(smaAllAbove == false && prevSMAAllAbove == true){
			if(CrossBelow(Close,SMA(20),1)){
				//ExitLong();
				//bought = false;
			}
			
			prevSMAAllAbove = smaAllAbove;
			prevSMAAllBelow = smaAllBelow;
			
        }
		
		private bool smaAllBelow(){
			return (SMA(5)[0]<SMA(20)[0]) && (SMA(20)[0]<SMA(30)[0]) && (SMA(30)[0]<SMA(90)[0]);
		}
		
		private bool smaAllAbove(){
			return (SMA(5)[0]>SMA(20)[0]) && (SMA(20)[0]>SMA(30)[0]) && (SMA(30)[0]>SMA(90)[0]);
		}

        #region Properties
        [Description("")]
        [GridCategory("Parameters")]
        public int MyInput0
        {
            get { return myInput0; }
            set { myInput0 = Math.Max(1, value); }
        }
        #endregion
    }
}
