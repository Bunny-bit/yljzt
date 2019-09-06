import React from 'react';
import { connect } from 'dva';
import styles from './Home.css';
import { Link } from 'dva/router';
import Chat from './modal/Chat';
import Searchfriend from './modal/Searchfriend';

import BindingThirdParty from './modal/BindingThirdParty';
import {
	Layout,
	Menu,
	Icon,
	Select,
	Badge,
	Avatar,
	Dropdown,
	Modal,
	Button,
	Input,
	Upload,
	Form,
	Popover,
	Switch,
	Checkbox,
	Spin,
	Tooltip,
	Row,
	Col
} from 'antd';
import UserLogin from './modal/UserLogin';
import { remoteUrl } from './../../utils/url';
const { Header, Content, Footer, Sider } = Layout;
const SubMenu = Menu.SubMenu;
const Option = Select.Option;
const FormItem = Form.Item;
const formItemLayout = {
	labelCol: { span: 4 },
	wrapperCol: { span: 8 }
};
const create = Form.create;

import notificationImage from '../../assets/notification.svg';
import chatImage from '../../assets/chat.svg';

class Home extends React.Component {
	componentDidMount() {
		this.props.dispatch({
			type: 'home/initialize'
		});
	}
	render() {
		const {
			dispatch,
			title,
			menus,
			mintalk,
			result,
			collapsed,
			loading,
			visible,
			children,
			touxiang,
			changePassword,
			changeInformation,
			openChat,
			havemessage,
			openSearchFriend,
			form,
			skins,
			notification,
			visibleNotification,
			visibleNotificationPopover,
			notificationSettings,
			friends,
			chatLoading,
			shouldChangePasswordOnNextLogin,
			thirdPartyVisible,
			messageor
		} = this.props;
		// less.watch();
		const { getFieldDecorator } = form;

		function toggle() {
			dispatch({
				type: 'home/setState',
				payload: {
					collapsed: !collapsed
				}
			});
		}

		function handleChange(value) {}

		function showModal() {
			dispatch({
				type: 'home/setState',
				payload: {
					changePassword: !changePassword
				}
			});
		}

		function handleConfirmPassword(rule, value, callback) {
			if (value && value !== form.getFieldValue('newPassword')) {
				callback('两次输入不一致！');
			}

			// Note: 必须总是返回一个 callback，否则 validateFieldsAndScroll 无法响应
			callback();
		}

		function clearNewPassword() {
			form.resetFields([ 'newPassword1' ]);
		}

		function handleOk(e) {
			e.preventDefault();
			form.validateFields([ 'currentPassword', 'newPassword', 'newPassword1' ], (err, values) => {
				if (!err) {
					dispatch({
						type: 'home/changepassword',
						payload: {
							currentPassword: values.currentPassword,
							newPassword: values.newPassword
						}
					});
				}
			});
		}

		function handleOkConfirm(e) {
			e.preventDefault();
			form.validateFields([ 'name', 'emailAddress', 'phoneNumber' ], (err, values) => {
				if (!err) {
					if (values.name == undefined) {
						values.name = result.user.name;
					}
					if (values.emailAddress == undefined) {
						values.emailAddress = result.user.emailAddress;
					}
					if (values.phoneNumber == undefined) {
						values.phoneNumber = result.user.phoneNumber;
					}
					dispatch({
						type: 'home/changession',
						payload: values
					});
				}
			});
		}

		function handleOkNotification(e) {
			e.preventDefault();
			form.validateFields(notificationSettings.notifications.map((n) => n.name), (err, values) => {
				if (!err) {
					var notifications = notificationSettings.notifications.map((n) => {
						return { name: n.name, isSubscribed: eval('values.' + n.name) };
					});
					dispatch({
						type: 'home/updateNotificationSettings',
						payload: {
							receiveNotifications: notificationSettings.receiveNotifications,
							notifications: notifications
						}
					});
				}
			});
		}

		function handleCancel() {
			dispatch({
				type: 'home/setState',
				payload: {
					changePassword: !changePassword
				}
			});
		}

		function handleCancel2() {
			dispatch({
				type: 'home/setState',
				payload: {
					changePassword: !changePassword
				}
			});
		}

		function handleCancel3() {
			dispatch({
				type: 'home/setState',
				payload: {
					changeInformation: !changeInformation
				}
			});
		}

		function handleCancelNotification() {
			dispatch({
				type: 'home/setState',
				payload: {
					visibleNotification: !visibleNotification
				}
			});
		}

		function recursion(dataSource) {
			dataSource.sort((l, r) => l.order - r.order);
			return dataSource.map((menu, index) => {
				if (menu.items && menu.items.length) {
					return (
						<SubMenu
							key={menu.name}
							title={
								<span>
									{menu.icon ? <Icon type={menu.icon} /> : null}
									<span>{menu.displayName}</span>
								</span>
							}
						>
							{recursion(menu.items)}
						</SubMenu>
					);
				} else {
					if (menu.target == '_blank') {
						return (
							<Menu.Item key={menu.name}>
								<a href={menu.url} target={menu.target}>
									{menu.icon ? <Icon type={menu.icon} /> : null}
									<span>{menu.displayName}</span>
								</a>
							</Menu.Item>
						);
					} else {
						return (
							<Menu.Item key={menu.url}>
								<Link to={menu.url} target={menu.target}>
									{menu.icon ? <Icon type={menu.icon} /> : null}
									<span>{menu.displayName}</span>
								</Link>
							</Menu.Item>
						);
					}
				}
			});
		}

		function showUserLoginModal() {
			dispatch({
				type: 'userLogin/getUserLogins',
				payload: {
					current: 1,
					pageSize: 10
				}
			});
		}

		function changemy() {
			dispatch({
				type: 'home/setState',
				payload: {
					changeInformation: !changeInformation
				}
			});
		}

		function setAllNotificationsAsRead() {
			dispatch({
				type: 'home/setAllNotificationsAsRead',
				payload: {}
			});
		}

		function handleVisibleNotificationPopoverChange(visible) {
			dispatch({
				type: 'home/setState',
				payload: {
					visibleNotificationPopover: visible
				}
			});
		}
		function loginoutmy() {
			dispatch({
				type: 'home/logoutmy',
				payload: {}
			});
		}
		function showchat() {
			dispatch({
				type: 'home/setState',
				payload: {
					openChat: true,
					mintalk: true,
					havemessage: false
				}
			});
			dispatch({
				type: 'home/getfriends',
				payload: {
					friends: friends
				}
			});
		}
		function changeSkinHandle(value) {
			dispatch({
				type: 'home/changeStorageAndUiColor',
				payload: value
			});
		}
		function bindingThirdParty() {
			dispatch({
				type: 'thirdpartybinding/getBindingThirdPartyList',
				payload: {}
			});
		}
		const menu = (
			<Menu>
				<Menu.Item>
					<a onClick={showModal}>
						<span>修改密码</span>
					</a>
				</Menu.Item>
				<Menu.Item>
					<a onClick={changemy}>
						<span>修改个人信息</span>
					</a>
				</Menu.Item>
				{thirdPartyVisible ? (
					<Menu.Item>
						<a onClick={bindingThirdParty}>
							<span>第三方账号绑定</span>
						</a>
					</Menu.Item>
				) : null}
				<Menu.Item>
					<a onClick={showUserLoginModal}>
						<span>登录记录</span>
					</a>
				</Menu.Item>
				<Menu.Item>
					<a onClick={loginoutmy}>
						<span>退出登录</span>
					</a>
				</Menu.Item>
			</Menu>
		);
		const uploadButton = (
			<div>
				<Icon type="plus" />
				<div className="ant-upload-text">Upload</div>
			</div>
		);
		const skinPopover = (
			<div className={styles.skinListContainer}>
				<Row>
					{skins.map((n, i) => (
						<Col span={4} key={i}>
							<a
								style={{ color: n.value, marginRigt: '10px' }}
								onClick={() => changeSkinHandle(n.value)}
								key={i}
							>
								<span>■</span>
								<span>{n.name}</span>
							</a>
						</Col>
					))}
				</Row>
			</div>
		);
		function mintalkclose() {
			dispatch({
				type: 'home/setState',
				payload: {
					mintalk: true,
					openChat: true,
					havemessage: false,
					display: false
				}
			});
		}
		function formatMsgTime(timespan) {
			var dateTime = new Date(timespan);

			var year = dateTime.getFullYear();
			var month = dateTime.getMonth() + 1;
			var day = dateTime.getDate();
			var hour = dateTime.getHours();
			var minute = dateTime.getMinutes();
			var second = dateTime.getSeconds();
			var now = new Date();
			var now_new = Date.parse(now.toDateString()); //typescript转换写法

			var milliseconds = 0;
			var timeSpanStr;

			milliseconds = now_new - timespan;

			if (milliseconds <= 1000 * 60 * 1) {
				timeSpanStr = '刚刚';
			} else if (1000 * 60 * 1 < milliseconds && milliseconds <= 1000 * 60 * 60) {
				timeSpanStr = Math.round(milliseconds / (1000 * 60)) + '分钟前';
			} else if (1000 * 60 * 60 * 1 < milliseconds && milliseconds <= 1000 * 60 * 60 * 24) {
				timeSpanStr = Math.round(milliseconds / (1000 * 60 * 60)) + '小时前';
			} else if (1000 * 60 * 60 * 24 < milliseconds && milliseconds <= 1000 * 60 * 60 * 24 * 15) {
				timeSpanStr = Math.round(milliseconds / (1000 * 60 * 60 * 24)) + '天前';
			} else if (milliseconds > 1000 * 60 * 60 * 24 * 15 && year == now.getFullYear()) {
				timeSpanStr = month + '-' + day + ' ' + hour + ':' + minute;
			} else {
				timeSpanStr = year + '-' + month + '-' + day + ' ' + hour + ':' + minute;
			}
			return timeSpanStr;
		}

		const notificationsList = (
			<div>
				<div>
					{notification.items.map((n, i) => (
						<div key={n.id} className={styles.notificationsContent}>
							<div>{n.notification.data.message}</div>
							<div>
								<span className={styles.notificationsDate}>
									{formatMsgTime(new Date(n.notification.creationTime))}
								</span>
								<span>{n.state == 0 ? '未读' : '已读'}</span>
							</div>
						</div>
					))}
				</div>
				<div className={styles.notificationsFooter}>
					<a href="#notification">查看全部通知</a>
				</div>
			</div>
		);

		const props = {
			name: 'file',
			action: `${remoteUrl}/Profile/ChangeProfilePicture`,
			showUploadList: false,
			headers: {
				authorization: 'Bearer ' + window.localStorage.getItem('token')
			},
			beforeUpload(file) {
				//console.log(file);
				var isTypeTrue = file.type === 'image/jpeg' || file.type === 'image/png';
				if (!isTypeTrue) {
					Modal.warning({
						title: '请上传JPG/PNG类型的文件'
					});
				}
				return isTypeTrue;
			},
			onChange(info) {
				if (info.file.status !== 'uploading') {
					//console.log(info.file, info.fileList);
				}
				if (info.file.status === 'done') {
					if (info.fileList[info.fileList.length - 1].response.error) {
						Modal.error({
							title: '上传失败',
							content: info.fileList[info.fileList.length - 1].response.error.message
						});
						return;
					}
					Modal.success({
						title: '上传成功'
					});
				} else if (info.file.status === 'error') {
					Modal.error({
						title: '上传失败'
					});
				}
			}
		};

		// const { previewVisible, previewImage, fileList } = this.state;
		return (
			<Layout style={{ height: '100%' }}>
				<Sider
					trigger={null}
					collapsible
					collapsed={collapsed}
					onCollapse={(collapsed, type) => {
						//展开-收起时的回调函数，有点击 trigger 以及响应式反馈两种方式可以触发
						toggle();
						// console.log(collapsed);
						// console.log(type);
					}}
					breakpoint="lg"
					style={{ background: '#fff' }}
				>
					<div className={styles.logo}>
						<img src={`${remoteUrl}/Logo/GetLogoPicture`} />
					</div>
					<Menu mode="inline" defaultSelectedKeys={[ this.props.location.pathname ]}>
						{recursion(menus)}
					</Menu>
				</Sider>
				<Layout>
					<Header className={styles.headerD}>
						<Icon
							className={styles.trigger}
							type={collapsed ? 'menu-unfold' : 'menu-fold'}
							onClick={toggle}
						/>
						<div className={styles.headbox}>
							<Popover content={skinPopover} trigger="hover" placement="bottom">
								<a className="ant-dropdown-link">
									切换主题 <Icon type="down" />
								</a>
							</Popover>
							<Popover
								visible={visibleNotificationPopover}
								onVisibleChange={handleVisibleNotificationPopoverChange}
								content={notificationsList}
								title={
									<span>
										<a onClick={setAllNotificationsAsRead}>全部标记为已读</a>
										<a
											style={{ float: 'right' }}
											onClick={() => {
												dispatch({
													type: 'home/getNotificationSettings'
												});
												dispatch({
													type: 'home/setState',
													payload: {
														visibleNotification: !visibleNotification,
														visibleNotificationPopover: false
													}
												});
											}}
										>
											设置
										</a>
									</span>
								}
								trigger="click"
								placement="bottom"
							>
								<Badge count={notification.unreadCount} className={styles.badeg}>
									<a>
										<img style={{ width: '24px', height: '24px' }} src={notificationImage} />
									</a>
								</Badge>
							</Popover>
							<div className={styles.headpor}>
								<Avatar
									src={result.user ? `${remoteUrl}` + result.user.profile : ''}
									size="small"
									className={styles.head}
								/>
								<Dropdown overlay={menu}>
									<a className="ant-dropdown-link">
										{result.user ? result.user.name : ''}
										<Icon type="down" />
									</a>
								</Dropdown>
							</div>
							<a style={{ lineHeight: 1 }}>
								{chatLoading ? (
									<Tooltip placement="bottomRight" title={'正在连接聊天服务器'}>
										<Spin />
									</Tooltip>
								) : (
									<div>
										<Badge count={havemessage && messageor ? messageor.side == 2 ? 'new' : '' : ''}>
											<img
												onClick={showchat}
												style={{ width: '26px', height: '26px' }}
												src={chatImage}
											/>
										</Badge>
										{openChat ? <Chat /> : null}
										{openSearchFriend ? <Searchfriend /> : null}
									</div>
								)}
							</a>
						</div>
					</Header>
					<Content className={styles.contentC}>
						<div className={styles.contentD}>
							<div className={styles.bigbox}>
								<div className={styles.bighead}>{title}</div>
								<div className={styles.normal}>{children}</div>
							</div>
						</div>
					</Content>
					<Footer className={styles.contentF}>GAIA ©2017 Created by 青才信息科技有限公司</Footer>
				</Layout>

				{changePassword || shouldChangePasswordOnNextLogin ? (
					<Modal
						visible={changePassword || shouldChangePasswordOnNextLogin}
						title={shouldChangePasswordOnNextLogin ? '第一次登录需要修改密码' : '修改密码'}
						closable={!shouldChangePasswordOnNextLogin}
						onOk={handleOk}
						onCancel={handleCancel}
						width="450px"
						footer={[
							shouldChangePasswordOnNextLogin ? null : (
								<Button key="back" onClick={handleCancel}>
									取消
								</Button>
							),
							<Button key="submit" type="primary" loading={loading} onClick={handleOk}>
								确认
							</Button>
						]}
					>
						<Form>
							<FormItem label="旧密码：" labelCol={{ span: 6 }} wrapperCol={{ span: 15 }}>
								{getFieldDecorator('currentPassword', {
									initialValue: '',
									rules: [ { required: true, message: '请填写旧密码' } ]
								})(<Input type="password" />)}
							</FormItem>
							<FormItem label="新密码：" labelCol={{ span: 6 }} wrapperCol={{ span: 15 }}>
								{getFieldDecorator('newPassword', {
									initialValue: '',
									rules: [ { required: true, message: '请填写新密码' } ]
								})(<Input type="password" onChange={clearNewPassword} />)}
							</FormItem>
							<FormItem label="确认密码：" labelCol={{ span: 6 }} wrapperCol={{ span: 15 }}>
								{getFieldDecorator('newPassword1', {
									initialValue: '',
									rules: [
										{ required: true, message: '请填写确认密码' },
										{
											validator: handleConfirmPassword
										}
									]
								})(<Input type="password" />)}
							</FormItem>
						</Form>
					</Modal>
				) : null}
				{changeInformation ? (
					<Modal
						visible={changeInformation}
						title="修改个人信息"
						width="550"
						onCancel={handleCancel3}
						footer={[
							<Button key="back" onClick={handleCancel3}>
								取消
							</Button>,
							<Button key="submit" type="primary" loading={loading} onClick={handleOkConfirm}>
								确认
							</Button>
						]}
					>
						<div style={{ height: 200 }}>
							<div className={styles.upface}>
								<header className={styles.uphead}>头像</header>
								<Upload {...props}>
									<Button
										type="ghost"
										className={styles.upbox}
										style={{
											backgroundSize: 'contain',
											backgroundImage:
												'url(' + (result.user ? `${remoteUrl}` + result.user.profile : '') + ')'
										}}
									>
										<Icon type="plus" style={{ fontSize: 30, color: '#c6c8cb' }} />
									</Button>
								</Upload>
								<div className={styles.updateDescribe}>30KB内的JPG/PNG图片</div>
							</div>
							<div className={styles.upshow}>
								<div>
									<header className={styles.uphead}>个人信息</header>
								</div>
								<Form>
									<FormItem
										label="姓名："
										labelCol={{ span: 6 }}
										wrapperCol={{ span: 18 }}
										style={{ marginTop: 20 }}
									>
										{getFieldDecorator('name', {
											initialValue: result.user ? result.user.name : ''
										})(<Input />)}
									</FormItem>
									<FormItem label="邮箱地址：" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
										{getFieldDecorator('emailAddress', {
											initialValue: result.user ? result.user.emailAddress : ''
										})(<Input />)}
									</FormItem>
									<FormItem label="手机号码：" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
										{getFieldDecorator('phoneNumber', {
											initialValue: result.user ? result.user.phoneNumber : ''
										})(<Input />)}
									</FormItem>
								</Form>
							</div>
						</div>
					</Modal>
				) : null}
				<Modal
					visible={visibleNotification}
					title="通知设置"
					onOk={handleOkNotification}
					onCancel={handleCancelNotification}
					width="450px"
					footer={[
						<Button key="back" onClick={handleCancelNotification}>
							取消
						</Button>,
						<Button key="submit" type="primary" loading={loading} onClick={handleOkNotification}>
							确认
						</Button>
					]}
				>
					<Form>
						<h2>接收通知</h2>
						<FormItem help="这个选项可以用来完全启用/禁用接收通知。" labelCol={{ span: 6 }} wrapperCol={{ span: 15 }}>
							<Switch
								checked={notificationSettings.receiveNotifications}
								onChange={(chenked) => {
									dispatch({
										type: 'home/setNotificationSettingsState',
										payload: {
											receiveNotifications: chenked
										}
									});
								}}
							/>
						</FormItem>
						<h2>通知类型</h2>
						{notificationSettings.receiveNotifications ? (
							''
						) : (
							<div style={{ color: '#ed6b75' }}>
								<br />您完全禁用接收通知,您可以启用它并选择您想收到通知类型。<br />
								<br />
							</div>
						)}
						{notificationSettings.notifications.map((n, i) => (
							<div key={n.name}>
								{n.title ? <h2>{n.title}</h2> : ''}
								<FormItem help={n.description}>
									{getFieldDecorator(n.name, {
										initialValue: n.isSubscribed,
										valuePropName: n.isSubscribed ? 'checked' : 'unchecked'
									})(
										<Checkbox disabled={!notificationSettings.receiveNotifications}>
											{n.displayName}
										</Checkbox>
									)}
								</FormItem>
							</div>
						))}
					</Form>
				</Modal>
				<UserLogin />
				<div
					className={styles.mintalkbox}
					style={mintalk ? { display: 'none' } : { display: 'block' }}
					onClick={mintalkclose}
				>
					<div
						className={
							havemessage && messageor ? messageor.side == 2 ? (
								styles.mintalkcallget
							) : (
								styles.mintalkcall
							) : (
								styles.mintalkcall
							)
						}
					>
						{havemessage && messageor ? messageor.side == 2 ? '您有新消息！' : 'loading...' : 'loading...'}
					</div>
				</div>
				<BindingThirdParty />
			</Layout>
		);
	}
}
Home = connect((state) => {
	return {
		...state.home,
		thirdPartyVisible:
			state.indexpage.setting.alipayIsEnabled ||
			state.indexpage.setting.qqIsEnabled ||
			state.indexpage.setting.weiboIsEnabled ||
			state.indexpage.setting.weixinOpenIsEnabled
	};
})(Home);
export default create()(Home);
