import fetch from 'dva/fetch';
import {remoteUrl} from './url';
import NProgress from 'nprogress';
import 'nprogress/nprogress.css';
import {notification} from 'antd';

function parseJSON(response) {
  let jsonObj = response.json();
  //500处理
  jsonObj.then((data) => {
    if (!data.success && data.error) {
      notification.error({
        message: response.status + '  ' + data.error.message,
        description: data.error.details && data.error.details.length > 200 ? data.error.details.substring(0, 200) : data.error.details,
      });
    }
  });
  return jsonObj;
}

function checkStatus(response) {
  if (response.status >= 200 && response.status < 300 || response.status == 500) {
    return response;
  }
  if (response.status == 401) {
    const error = new Error(401, '没有权限访问接口!');
    error.status = 401;
    throw error;
  }
  return response;
}

function nprogress(response) {
  NProgress.done(); //顶部进度条结束
  return response;
}

/**
 * Requests a URL, returning a promise.
 *
 * @param  {string} url       The URL we want to request
 * @param  {object} [options] The options we want to pass to "fetch"
 * @return {object}           An object containing either "data" or "err"
 */
export default function request(url, options) {
  NProgress.start();  //顶部进度条开始
  const opts = {...options};
  let token = window.localStorage.getItem('token');
  //请求头
  opts.headers = {
    'Content-Type': 'application/json; charset=UTF-8',
    'Authorization': 'Bearer ' + token,
    ...opts.headers,
  };
  //请求方法
  let method = opts.method.toLowerCase();
  // console.log(JSON.stringify(opts.body));
  return fetch(`${remoteUrl}${url}`, {
    ...opts,
    body: method == 'get' || method == 'delete' ? null : JSON.stringify(opts.body),
    withCredentials: true,
    credentials: "include",
  })
    .then(nprogress)
    .then(checkStatus)
    .then(parseJSON)

}
