import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Link } from 'dva/router';
import { Form, Icon, Input, Button, Row, Col, notification, message, Modal } from 'antd';
import styles from './Activation.css';
const FormItem = Form.Item;
const create = Form.create;
import { remoteUrl } from './../../../utils/url';
import * as verify from './../../../utils/verify';
import DragVerification from '../../DragVerification/DragVerification';
import Geetest from '../../Geetest/Geetest';

class Email extends Component {
	constructor(props) {
		super(props);
		this.state = {
			emailRandom: false,
			second: 60, //倒计时
			btn1: false,
			btn11: 0,
			btn2: false,
			btnname: '获取验证码',
			successClass: false
		};
		this.handleSubmit = this.handleSubmit.bind(this);
		this.sendEmail = this.sendEmail.bind(this);
	}

	handleSubmit(e) {
		e.preventDefault();
		const { dispatch, form } = this.props;
		form.validateFields((err, values) => {
			if (!err) {
				if (!verify.valiEnpty(values.email)) {
					message.error('请输入正确的邮箱码');
					return;
				}
				if (!verify.validatecode(values.code)) {
					message.error('请填写正确的验证码');
					return;
				}
				dispatch({
					type: 'sendemail/confirmEmailByCode',
					payload: {
						...values,
						sufn: () => {
							// notification.success({
							//   message: '验证成功',
							//   description: '邮箱验证成功',
							// });
							Modal.success({
								title: '验证成功',
								content: '邮箱验证成功',
								maskClosable: true,
								okText: '确定'
							});
							this.setState({
								successClass: true
							});
						},
						fafn: () => {
							// notification.error({
							//   message: '验证失败',
							//   description: '邮箱验证失败',
							// });
							Modal.error({
								title: '验证失败',
								content: '邮箱验证失败',
								maskClosable: true,
								okText: '确定'
							});
						}
					}
				});
			}
		});
	}

	sendEmail() {
		const { dispatch, form } = this.props;
		const email = form.getFieldValue('email');
		const captcha = form.getFieldValue('captcha');
		if (!verify.valiEnpty(email)) {
			message.error('请输入邮箱地址');
			return;
		}
		if (!verify.valiEnpty(captcha)) {
			message.error('请填点击获取验证码');
			return;
		}
		this.setState({
			btn1: true,
			btn11: 1,
			btnname: '一分钟后重试'
		});
		var _this = this;
		setTimeout(function() {
			_this.setState({
				btn1: false,
				btnname: '获取验证码'
			});
			_this.props.dispatch({
				type: 'geetest/reset'
			});
		}, 60000);
		dispatch({
			type: 'sendemail/sendConfirmEmailCode',
			payload: {
				email: email,
				captcha: captcha,
				sufn: () => {
					// notification.success({
					//   message: '邮件验证码已发送',
					//   description: '请查看后输入您收到的邮件验证码',
					// });
					Modal.success({
						title: '邮件验证码已发送',
						content: '请查看后输入您收到的邮件验证码',
						maskClosable: true,
						okText: '确定'
					});
					this.setState({
						second: 60,
						btn11: 2
					});
					this.timer1 = setInterval(() => {
						if (this.state.second == 0) {
							this.setState({
								second: 60,
								btn1: false,
								btn11: 0,
								smsRandom: null
							});
							this.timer1 && clearInterval(this.timer1);
						} else {
							this.setState({
								second: this.state.second - 1
							});
						}
					}, 1000);
				},
				fafn: () => {
					// notification.error({
					//   message: '邮件验证码发送失败',
					//   description: '请稍后再试',
					// });
					Modal.error({
						title: '邮件验证码发送失败',
						content: '请稍后再试',
						maskClosable: true,
						okText: '确定'
					});
					this.setState({
						second: 60,
						btn1: false,
						btn11: 0
					});
				}
			}
		});
	}

	renderEmail() {
		let btnStr = '';
		if (this.state.btn11 == 0) {
			//没发或失败
			btnStr = '获取验证码';
		} else if (this.state.btn11 == 1) {
			//网络请求
			btnStr = '验证码发送中...';
		} else if (this.state.btn11 == 2) {
			//网络请求结束，成功
			btnStr = this.state.second + '秒';
		}
		return btnStr;
	}

	componentWillUnmount() {
		//离开界面执行
		this.timer1 && clearTimeout(this.timer1);
	}

	render() {
		const { loading, form } = this.props;
		const { getFieldDecorator } = form;
		return (
			<div className={styles.normal}>
				<header className={styles.headerbox}>
					<span className={styles.headcol}>邮箱验证</span>
					{this.props.goToCode == 2 ? (
						<Link to="/" className={styles.headback}>
							返回登录
						</Link>
					) : null}
					{/*<Link to='/' className={styles.headback}>返回登录</Link>*/}
				</header>
				<Form onSubmit={this.handleSubmit}>
					<FormItem>
						<div className={styles.colorSize}>邮箱地址：</div>
						{getFieldDecorator('email', {
							rules: [ { required: true, message: '请输入正确的邮箱地址！', type: 'email' } ]
						})(<Input placeholder="请输入邮箱地址" />)}
					</FormItem>
					{/* <FormItem>
            <div className={styles.colorSize}>图形验证码：</div>
            <Row gutter={8}>
              <Col span={14}>
                {getFieldDecorator('captcha', {
                  // rules: [{required: true, message: '请输入图形验证码！'}],
                })(
                  <Input placeholder="请输入图形验证码"/>
                )}
              </Col>
              <Col span={10} className={styles.imgCol}>
                {
                  this.state.emailRandom ?
                    <a className={styles.imgSMS} onClick={() => {
                      this.setState({emailRandom: Math.random()});
                    }}><img src={`${remoteUrl}/Captcha/GetCaptchaImage?t=${this.state.emailRandom}`}/></a>
                    :
                    <Button type="primary" disabled={this.state.btn1} className={styles.smsBtn}
                            onClick={() => {
                              this.setState({emailRandom: Math.random()});
                            }}>
                      获取图片验证码
                    </Button>
                }
              </Col>
            </Row>
          </FormItem> */}
					<FormItem>
						<div className={styles.colorsize}>验证码：</div>
						{this.state.smsRandom ? (
							<center>
								{getFieldDecorator('captcha', {
									rules: [ { required: true, message: '请点击验证！' } ]
								})(<Geetest captchaHtmlId="embed-captcha1" />)}
							</center>
						) : (
							<Button
								type="primary"
								disabled={this.state.btn1}
								className={styles.smsBtn}
								onClick={() => {
									this.setState({ smsRandom: Math.random() });
								}}
							>
								获取图片验证码
							</Button>
						)}
					</FormItem>
					<FormItem>
						<div className={styles.colorSize}>邮件验证码：</div>
						<Row gutter={8}>
							<Col span={14}>
								{getFieldDecorator('code', {
									rules: [ { required: true, message: '请输入邮件验证码！' } ]
								})(<Input placeholder="请输入邮件验证码" />)}
							</Col>
							<Col span={10}>
								<Button
									type="primary"
									disabled={this.state.btn1}
									className={styles.smsBtn}
									onClick={this.sendEmail}
								>
									{this.state.btnname}
								</Button>
							</Col>
						</Row>
					</FormItem>
					<Button
						type="primary"
						disabled={this.state.successClass}
						icon={this.state.successClass ? 'check-circle-o' : ''}
						htmlType="submit"
						className={this.state.successClass ? styles.login1 : styles.login}
						loading={loading}
					>
						验证{this.state.successClass ? '已通过' : ''}
					</Button>
				</Form>
			</div>
		);
	}
}

Email = connect((state) => {
	return {
		...state.Sendemail,
		loading: state.loading.effects['sendemail/confirmEmailByCode']
	};
})(Email);

export default create()(Email);
