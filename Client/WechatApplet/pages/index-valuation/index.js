// pages/index-valuation/index.js
const util = require('../../utils/util.js');
Page({

  /**
   * 页面的初始数据
   */
  data: {
    indexStyle: 0,/* 0:全部,1:宽基,2:行业,3:策略,4:全球 */
    showLoading: true,
    lastUpdateTime:"",
    indexValuations:[]
  },
  loadIndexValuations(){
    var that=this;
    wx.request({
      url: util.BaseUrl+'v1/api/Index',
      success(res){
        that.setData({
          showLoading:false,
          lastUpdateTime:res.data.lastUpdateTime,
          indexValuations:res.data.valuations
        });
        
        /* 每次页面加载时，数据直接缓存本地*/
        wx.setStorageSync('allIndexValuations', res.data.valuations);
      }
    })
  },
  swichNav(e) {
    var indexStyle = e.target.dataset.indexStyle;
    var allIndexValuations = wx.getStorageSync('allIndexValuations');
    var currentIndexValuations=allIndexValuations;
    if(indexStyle!=0){
      currentIndexValuations=allIndexValuations.filter(item=>item.style==indexStyle);
    }
    
    this.setData({
      indexStyle: indexStyle,
      indexValuations:currentIndexValuations
    });
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad(options) {
    this.loadIndexValuations();
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady() {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow() {
    if (typeof this.getTabBar === 'function' &&
    this.getTabBar()) {
    this.getTabBar().setData({
      indexStyle: 0
    })
  }
  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide() {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload() {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh() {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom() {
   
  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage() {

  }
})