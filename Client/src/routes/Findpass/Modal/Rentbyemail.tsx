import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button, Steps} from 'antd';
import styles from '../Findpass.css';
const Step = Steps.Step;
const create = Form.create;
const FormItem = Form.Item;
function Rentbyemail({dispatch, form, current}) {
  const {getFieldDecorator} = form;
  let val = false;

  function getValByUrl(key) {

    let Reg = new RegExp(`${key}=(\\w*)`);
    // let Reg = /code=(\w*)/;  //正则
    let info = location.href.match(Reg);
    if (info && info.length == 2) {
      val = info[1];
    }
    // console.log(`${key},${val}`);
    return val;
  }

  function handleSubmit(e) {
    e.preventDefault();
    getValByUrl('key');
    form.validateFields(['password', 'password1'], (err, values) => {
      if (!err) {
        if (values.password == values.password1) {
          dispatch({
            type: 'rentbyemail/sentpass',
            payload: {password: values.password, secretKey: val}
          });
        } else {
          notification.error({
            message: '修改失败',
            description: '两次密码不一致'
          });
        }
      }
    });
  }

  return (
    <Form onSubmit={handleSubmit} className={`${styles.formbox} login-form`}>
      <FormItem>
        <div className={styles.colorsize}>新密码：</div>
        {getFieldDecorator('password', {
          rules: [{required: true, message: '请输入邮箱地址！'}],
        })(
          <Input prefix={<Icon type="user" style={{fontSize: 13}}/>} placeholder="请输入新密码"
                 style={{marginTop: '5px', height: 40}} type="password"/>
        )}
      </FormItem>
      <FormItem>
        <div className={styles.colorsize}>确认新密码：</div>
        {getFieldDecorator('password1', {
          rules: [{required: true, message: '请输入邮箱地址！'}],
        })(
          <Input prefix={<Icon type="user" style={{fontSize: 13}}/>} placeholder="请再次输入新密码"
                 style={{marginTop: '5px', height: 40}} type="password"/>
        )}
      </FormItem>
      <Button type="primary" className={styles.login} onClick={handleSubmit}>
        确定
      </Button>
    </Form>
  )
}
Rentbyemail = connect((state) => {
  return {
    ...state.rentbyemail,
  }
})(Rentbyemail);

export default create()(Rentbyemail);

