import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Link } from 'dva/router';
import { Form, Icon, Input, Button, Checkbox, message, Row, Col } from 'antd';
import styles from './Register.css';
import Logo from '../../assets/logologin.gif';
import { remoteUrl } from '../../utils/url';
const FormItem = Form.Item;
const create = Form.create;
import DragVerification from '../DragVerification/DragVerification';
import Geetest from '../Geetest/Geetest';
function Register({ dispatch, form, captcha, abled, hedhtml, thirdPartyToken }) {
	const { getFieldDecorator } = form;

	function refreshCaptcha() {
		dispatch({
			type: 'geetest/reset'
		});
	}

	function getcaptch(e) {
		e.preventDefault();
		form.validateFields([ 'phoneNumber', 'captcha' ], (err, values) => {
			if (!err) {
				let second = 60;
				let timer = setInterval(function() {
					if (second > 0) {
						second--;
						dispatch({
							type: 'register/setState',
							payload: {
								hedhtml: second
							}
						});
					} else {
						refreshCaptcha();
						dispatch({
							type: 'register/setState',
							payload: {
								hedhtml: '获取验证码',
								abled: false
							}
						});
						clearInterval(timer);
					}
				}, 1000);

				dispatch({
					type: 'register/setState',
					payload: {
						hedhtml: '一分钟后重试',
						second: 60,
						abled: true
					}
				});
				dispatch({
					type: 'register/sendPhoneNumberCode',
					payload: values
				});
			}
		});
	}

	function handleSubmit(e) {
		e.preventDefault();
		form.validateFields([ 'userName', 'phoneNumber', 'captchamess', 'password', 'remember' ], (err, values) => {
			if (!err) {
				console.log('Received values of form: ', values);
				if (values.remember) {
					dispatch({
						type: 'register/register',
						payload: {
							userName: values.userName,
							phoneNumber: values.phoneNumber,
							captcha: values.captchamess,
							password: values.password,
							token: thirdPartyToken
						}
					});
				} else {
					message.error('请仔细阅读用户协议');
				}
			}
		});
	}

	return (
		<div className={styles.navbox}>
			<div className={styles.logologin}>
				<img src={Logo} />
			</div>
			<div className={styles.indexbox}>
				<header className={styles.headerbox}>
					<span className={styles.headcol}>新用户注册</span>
					<Link to="/" className={styles.headback}>
						返回登录
					</Link>
				</header>
				<Form onSubmit={handleSubmit} className={`${styles.formbox} login-form`}>
					<FormItem>
						<div className={styles.colorsize}>用户名：</div>
						{getFieldDecorator('userName', {
							rules: [ { required: true, message: '请输入用户名！' } ]
						})(
							<Input
								prefix={<Icon type="user" style={{ fontSize: 13 }} />}
								placeholder="1-16个字符"
								style={{ marginTop: '5px', height: 40 }}
							/>
						)}
					</FormItem>
					<FormItem>
						<span className={styles.colorsize}>手机号：</span>
						{getFieldDecorator('phoneNumber', {
							rules: [ { required: true, message: '请输出手机号码！' } ]
						})(
							<Input
								prefix={<Icon type="phone" style={{ fontSize: 13 }} />}
								placeholder="手机号"
								style={{ marginTop: '5px', height: 40 }}
							/>
						)}
					</FormItem>
					{/* <FormItem>
						<span className={styles.colorsize}>验证码：</span>
						{getFieldDecorator('captcha', {
							rules: [ { required: true, message: '请输入验证码！' } ]
						})(
							<Row>
								<Col span={16}>
									<Input
										prefix={<Icon type="lock" style={{ fontSize: 13 }} />}
										type="captcha"
										placeholder="验证码"
										style={{ marginTop: '5px', height: 40 }}
									/>
								</Col>
								<Col span={8} style={{ textAlign: 'right' }}>
									<a onClick={refreshCaptcha}>
										<img src={captcha} style={{ marginTop: '5px', height: 40 }} />
									</a>
								</Col>
							</Row>
						)}
          </FormItem> */}
					{/*<FormItem>
							<span className={styles.colorsize}>验证码：</span>
							<center>
								{getFieldDecorator('captcha', {
									rules: [ { required: true, message: '请拖动图片验证！' } ]
								})(<DragVerification />)}
							</center>
						</FormItem>*/}
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
						{getFieldDecorator('captchamess', {
							rules: [ { required: true, message: '请输入短信验证码！' } ]
						})(
							<div style={{ height: '35px', marginTop: '5px' }}>
								<Input
									prefix={<Icon type="shake" style={{ fontSize: 13 }} />}
									placeholder="短信验证码"
									style={{ width: '230px', float: 'left', height: 40 }}
								/>
								<Button
									type="primary"
									htmlType="submit"
									style={{ float: 'left', marginLeft: '10px', width: '113px', height: 40 }}
									onClick={getcaptch}
									disabled={abled}
								>
									{hedhtml}
								</Button>
							</div>
						)}
					</FormItem>

					<FormItem>
						<span className={styles.colorsize}>密码：</span>
						{getFieldDecorator('password', {
							rules: [ { required: true, message: '请输入密码！' } ]
						})(
							<Input
								prefix={<Icon type="lock" style={{ fontSize: 13 }} />}
								type="password"
								placeholder="密码长度6-20位"
								style={{ marginTop: '5px', height: 40 }}
							/>
						)}
					</FormItem>
					<FormItem>
						{getFieldDecorator('remember', {
							valuePropName: 'checked',
							initialValue: false
						})(<Checkbox>我已阅读并接受</Checkbox>)}
						<a className="login-form-forgot" href="">
							《用户协议》
						</a>
					</FormItem>
					<Button type="primary" htmlType="submit" className={styles.login}>
						注册
					</Button>
				</Form>
				<Link to="/registerByEmail" style={{ color: '#537fdf', fontSize: '16px' }}>
					使用邮箱注册
				</Link>
			</div>
		</div>
	);
}
Register = connect((state) => {
	return {
		...state.register,
		thirdPartyToken: state.indexpage.thirdPartyToken
	};
})(Register);

export default create()(Register);
