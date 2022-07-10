Component({
  data: {
    selected: 0,
    color: "#707070",
    selectedColor: "#fcc21b",
    list: [{
      pagePath: "/pages/index-valuation/index",
      iconPath: "/images/icon_index_valuation_n.png",
      selectedIconPath: "/images/icon_index_valuation_y.png",
      text: "指数估值"
    }, {
      pagePath: "/pages/market-sentiment/index",
      iconPath: "/images/icon_market_sentiment_n.png",
      selectedIconPath: "/images/icon_market_sentiment_y.png",
      text: "市场情绪"
    }]
  },
  attached() {
  },
  methods: {
    switchTab(e) {
      const data = e.currentTarget.dataset
      wx.switchTab({
        url: data.path,
        selected:data.index
      })
    }
  }
})