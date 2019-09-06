import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button, Steps, Row, Col,notification} from 'antd';
import styles from '../Findpass.css';
import Geetest from '../../Geetest/Geetest';
const Step = Steps.Step;
const create = Form.create;
const FormItem = Form.Item;
function Callback({dispatch, form, hedhtml, abled,loading}) {
  const {getFieldDecorator} = form;


  function getcaptch(e) {

    e.preventDefault();
    form.validateFields(['phoneNumber', 'captcha'], (err, values) => {
      dispatch({
        type: 'callback/setState',
        payload: {
          hedhtml: '一分钟后重试',
          abled: true
        }
      });
      dispatch({
        type: 'callback/setlogin',
        payload: values
      });
    });
    setTimeout(function () {
      dispatch({
        type: 'callback/setState',
        payload: {
          hedhtml: '获取验证码',
          abled: false
        }
      });
    }, 60000)
  }



  function subpassword(e) {
    e.preventDefault();
    form.validateFields(['password', 'password1','phoneNumber','code'], (err, values) => {
      if (!err) {
        if (values.password == values.password1) {
          dispatch({
            type: 'callback/setlogin3',
            payload: {password: values.password, phoneNumber:values.phoneNumber, code:values.code}
          });
          dispatch({
            type: 'callback/setState',
            payload: {loading:true}
          });
        } else {
          notification.error({
            message: '修改失败',
            description: '两次密码不一致'
          });
        }
      }else{
        console.log(err)
      }
    });
  }
  return (
    <div>
      <div>
      <Form onSubmit={getcaptch} className={`${styles.formbox} login-form`}>
        <FormItem>
          <span className={styles.colorsize}>手机号：</span>
          {getFieldDecorator('phoneNumber', {
            rules: [{required: true, message: '请输出手机号码！'}],
          })(
            <Input prefix={<Icon type="phone" style={{fontSize: 13}}/>} placeholder="手机号"
                   style={{marginTop: '5px', height: 40}}/>
          )}
        </FormItem>
        <FormItem>
							<span className={styles.colorsize}>验证码：</span>
							<center>
								{getFieldDecorator('captcha', {
									rules: [ { required: true, message: '请点击验证！' } ]
								})(<Geetest />)}
							</center>
				</FormItem>
        <FormItem>
          <span className={styles.colorsize}>短信验证码：</span>
          {getFieldDecorator('code', {
            rules: [{required: true, message: '请输入短信验证码！'}],
          })(
            <div style={{height: '35px', marginTop: '5px'}}>
              <Input prefix={<Icon type="shake" style={{fontSize: 13}}/>} placeholder="短信验证码"
                     style={{width: '230px', float: 'left', height: 40}}/>
              <Button type="primary" htmlType="submit"
                      style={{float: 'left', marginLeft: '10px', width: '113px', height: 40}} onClick={getcaptch}
                      disabled={abled}>{hedhtml}</Button>
            </div>
          )}

        </FormItem>
        <FormItem>
          <div className={styles.colorsize}>新密码：</div>
          {getFieldDecorator('password', {
            rules: [{required: true, message: '请输入您的密码'}],
          })(
            <Input prefix={<Icon type="user" style={{fontSize: 13}}/>} placeholder="请输入新密码"
                   style={{marginTop: '5px', height: 40}} type="password"/>
          )}
        </FormItem>
        <FormItem>
          <div className={styles.colorsize}>确认新密码：</div>
          {getFieldDecorator('password1', {
            rules: [{required: true, message: '请再次确认您的密码'}],
          })(
            <Input prefix={<Icon type="user" style={{fontSize: 13}}/>} placeholder="请再次输入新密码"
                   style={{marginTop: '5px', height: 40}} type="password"/>
          )}
        </FormItem>
        <Button type="primary" className={styles.login} onClick={subpassword} loading={loading}>
          下一步
        </Button>
      </Form>
    </div>
      <Link to='/backknow' style={{color: '#537fdf', fontSize: '16px'}}>使用邮箱找回</Link>
    </div>
  )
}
Callback = connect((state) => {
  return {
    ...state.callback,
    loading: state.loading.effects['callback/setlogin3']
  }
})(Callback);

export default create()(Callback);
