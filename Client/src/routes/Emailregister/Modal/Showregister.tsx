import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button, Checkbox, message} from 'antd';
import styles from '../../Activation/Activation.css';
const FormItem = Form.Item;
const create = Form.create;
function Showregister({dispatch, form, thirdPartyToken}) {
  const {getFieldDecorator} = form;

  function handleSubmit(e) {
    e.preventDefault();
    form.validateFields((err, values) => {
      if (!err) {
        if (values.remember) {
          dispatch({
            type: 'emailregister/login',
            payload: {
              userName: values.userName,
              emailAddress: values.emailAddress,
              password: values.password,
              token: thirdPartyToken
            }
          });
        } else {
          message.error('请仔细阅读用户协议');
        }

      }
    })
  }

  return (
    <div>
      <Form onSubmit={handleSubmit} className={`${styles.formbox} login-form`}>
        <FormItem>
          <div className={styles.colorsize}>用户名：</div>
          {getFieldDecorator('userName', {
            rules: [{
              required: true, message: '请输入用户名！'},{
              pattern:'^[a-zA-Z0-9]{1,32}$', message: '用户名由1-32个英文字母或数字构成'
            }],
          })(
            <Input prefix={<Icon type="user" style={{fontSize: 13}}/>} placeholder="1-32个英文字母或数字"
                   style={{marginTop: '5px', height: 40}}/>
          )}
        </FormItem>
        <FormItem>
          <span className={styles.colorsize}>邮箱地址：</span>
          {getFieldDecorator('emailAddress', {
            rules: [{
              required: true, message: '请输入邮箱地址'
            },{
              pattern:'^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$', message: '请输入正确的邮箱地址'
            }],
          })(
            <Input prefix={<Icon type="mail" style={{fontSize: 13}}/>} placeholder="请输入邮箱地址"
                   style={{marginTop: '5px', height: 40}}/>
          )}
        </FormItem>
        <FormItem>
          <span className={styles.colorsize}>密码：</span>
          {getFieldDecorator('password', {
            rules: [{required: true, message: '请输入密码！'}],
          })(
            <Input prefix={<Icon type="lock" style={{fontSize: 13}}/>} type="password" placeholder="密码长度8-20位"
                   style={{marginTop: '5px', height: 40}}/>
          )}
        </FormItem>
        <FormItem>
          {getFieldDecorator('remember', {
            valuePropName: 'checked',
            initialValue: true,
          })(
            <Checkbox>我已阅读并接受</Checkbox>
          )}
          <a className="login-form-forgot" href="">《用户协议》</a>
        </FormItem>
        <Button type="primary" htmlType="submit" className={styles.login}>
          注册
        </Button>
      </Form>
      <Link to='/register' style={{color: '#537fdf', fontSize: '16px'}}>使用手机号注册</Link>
    </div>
  )
}
Showregister = connect((state) => {
  return {
    ...state.Showregister,
    thirdPartyToken: state.indexpage.thirdPartyToken
  }
})(Showregister);

export default create()(Showregister);
