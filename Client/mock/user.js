'use strict';

/**
 * user.js
 * Created by 李廷旭 on 2017/9/5 17:31
 * 描述: mock模拟后端数据
 */
import Mock from 'mockjs';

module.exports = {
  'GET /users' (req, res) {
    console.log(req.query);
    setTimeout(() => {
      let data = Mock.mock({
        // 属性 list 的值是一个数组，其中含有 1 到 10 个元素
        [`result|${req.query.pageSize}`]: [{
          // 属性 id 是一个自增数，起始值为 1，每次增 1
          'id|+1': 1,
          'age|5-99': 12,
          'name|1': ['李廷旭', '刘宏玺', '徐永', '李文祥', '凡尧', '葛俊成', '张三', '李四', '王武'],
          'email': '@EMAIL'
        }]
      });

      res.json({     //将请求json格式返回
        success: true,
        ...data,
        page: {PageIndex: req.query.PageIndex, PageSize: req.query.PageSize, TotalRecord: 88, AlsoData: true, TotalPages: 8},
        error: null,
        targetUrl: null,
        unAuthorizedRequest: false,
        __abp: true
      });
    }, 1000);
  }
};
