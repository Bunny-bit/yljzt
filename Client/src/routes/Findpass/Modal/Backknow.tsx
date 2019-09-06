import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button, Steps,Row,Col} from 'antd';
import styles from '../Findpass.css';
import Geetest from '../../Geetest/Geetest';
const Step = Steps.Step;
const create = Form.create;
const FormItem = Form.Item;
function Backkonw({dispatch, form,loading,hedhtml, abled}) {
  const {getFieldDecorator} = form;


  function handleSubmit(e) {

  }
  function getcaptch(e) {
    
        e.preventDefault();
        form.validateFields(['email', 'captcha'], (err, values) => {
          dispatch({
            type: 'backknow/setState',
            payload: {
              hedhtml: '一分钟后重试',
              abled: true
            }
          });
          dispatch({
            type: 'backknow/sentpass',
            payload: values
          });
        });
        setTimeout(function () {
          dispatch({
            type: 'backknow/setState',
            payload: {
              hedhtml: '获取验证码',
              abled: false
            }
          });
        }, 60000)
      }
  function next(e) {
    e.preventDefault();
    form.validateFields(['password', 'password1','email','code'], (err, values) => {
      if (!err) {
        if (values.password == values.password1) {
          dispatch({
            type: 'backknow/sentpass2',
            payload: {password: values.password, email:values.email, verificationCode:values.code}
          });
          dispatch({
            type: 'backknow/setState',
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
      <Form className={`${styles.formbox} login-form`}>
        <FormItem>
          <div className={styles.colorsize}>邮箱地址：</div>
          {getFieldDecorator('email', {
            rules: [{required: true, message: '请输入邮箱地址！'}],
          })(
            <Input prefix={<Icon type="layout" style={{fontSize: 13}}/>} placeholder="请输入邮箱地址"
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
          <span className={styles.colorsize}>邮箱验证码：</span>
          {getFieldDecorator('code', {
            rules: [{required: true, message: '请输入邮箱验证码！'}],
          })(
            <div style={{height: '35px', marginTop: '5px'}}>
              <Input prefix={<Icon type="exception" style={{fontSize: 13}}/>} placeholder="邮箱验证码"
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
        <Button type="primary" className={styles.login} onClick={next} loading={loading}>
          下一步
        </Button>
      </Form>
    </div>
      <Link to='/callback' style={{color: '#537fdf', fontSize: '16px'}}>使用手机号找回</Link>
    </div>
  )
}
Backkonw = connect((state) => {
  return {
    ...state.backknow,
    loading: state.loading.effects['backknow/sentpass2']
  }
})(Backkonw);

export default create()(Backkonw);
