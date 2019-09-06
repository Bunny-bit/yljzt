import React from 'react';
import { Modal, Button, Form, Input, Icon, Tabs, Popover, Badge, Tooltip } from 'antd';
const create = Form.create;
import { connect } from 'dva';
import styles from './Chat.css';
import { remoteUrl } from '../../../utils/url';
import $ from '../../../utils/jquery-vendor';
import moment from 'moment';
const Search = Input.Search;
const { TextArea } = Input;
const FormItem = Form.Item;
const TabPane = Tabs.TabPane;
const customPanelStyle = {
	borderRadius: 4,
	marginTop: 5,
	overflowY: 'auto',
	border: 'none'
};
function Chat({
	dispatch,
	openChat,
	form,
	showtexts,
	morenshow,
	friends,
	searchfriends,
	openSearchFriend,
	friendPagination,
	sendvalue,
	mybbtodo,
	allmessage,
	result,
	display,
	shownew
}) {
	const { getFieldDecorator } = form;
	let refDom;
	function talkOpen() {
		dispatch({
			type: 'home/getfriends',
			payload: {
				friends: friends
			}
		});
	}
	function talkClose() {
		dispatch({
			type: 'home/setState',
			payload: {
				openChat: !openChat,
				display: false
			}
		});
	}
	function sendme(e) {
		e.preventDefault();
		form.validateFields((err, values) => {
			form.setFieldsValue({ textshow: '' });
			dispatch({
				type: 'home/sendMessage',
				payload: {
					TenantId: sendvalue.friendTenantId,
					UserId: sendvalue.friendUserId,
					UserName: sendvalue.friendUserName,
					TenancyName: sendvalue.friendTenancyName,
					ProfilePictureId: sendvalue.friendProfilePictureId,
					Message: values.textshow
				}
			});
		});
	}
	function sendmymessage(e) {
		e.preventDefault();
		form.validateFields((err, values) => {
			form.setFieldsValue({ textshow: '' });
			dispatch({
				type: 'home/sendMessage',
				payload: {
					TenantId: sendvalue.friendTenantId,
					UserId: sendvalue.friendUserId,
					UserName: sendvalue.friendUserName,
					TenancyName: sendvalue.friendTenancyName,
					ProfilePictureId: sendvalue.friendProfilePictureId,
					Message: values.textshow
				}
			});
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

	function totalk(e) {
		dispatch({
			type: 'home/setState',
			payload: {
				mybbtodo: [],
				sendvalue: e,
				display: true
			}
		}),
			dispatch({
				type: 'home/getchatmessage',
				payload: {
					tenantId: e.friendTenantId,
					userId: e.friendUserId,
					minMessageId: null
				}
			});
	}
	function block() {
		dispatch({
			type: 'home/blockuser',
			payload: {
				userId: sendvalue.friendUserId,
				tenantId: sendvalue.friendTenantId
			}
		});
	}
	function unblock() {
		dispatch({
			type: 'home/unblockuser',
			payload: {
				userId: sendvalue.friendUserId,
				tenantId: sendvalue.friendTenantId
			}
		}),
			dispatch({
				type: 'home/getfriends',
				payload: {
					friends: friends
				}
			});
	}
	function closebox() {
		dispatch({
			type: 'home/setState',
			payload: {
				display: false
			}
		});
	}
	function mintalk() {
		dispatch({
			type: 'home/setState',
			payload: {
				openChat: !openChat,
				mintalk: !mintalk
			}
		});
	}
	function closetalk() {
		dispatch({
			type: 'home/setState',
			payload: {
				openChat: !openChat
			}
		});
	}
	const content = (
		<div>
			<p style={{ cursor: 'pointer' }}>
				{sendvalue.state == 1 ? <span onClick={block}>拉黑</span> : <span onClick={unblock}>取消拉黑</span>}
			</p>
		</div>
	);
	return (
		<div className={styles.talkshowmodal} style={display ? { width: 1000 } : { width: 400 }}>
			<div className={styles.talkheader}>
				<div className={styles.talktitle}>聊天框</div>
				<div className={styles.talkicon}>
					<Icon type="minus" onClick={mintalk} />
					<Icon type="close" style={{ marginLeft: 30 }} onClick={closetalk} />
				</div>
			</div>
			<div>
				<div className={styles.listbox}>
					<div className={styles.friendlist} style={display ? { width: '40%' } : { width: '400px' }}>
						<div className={styles.searchfriend}>
							<Search
								placeholder="搜索好友"
								style={{ width: '60%', margin: '10px 20px', fontSize: '20px' }}
								onSearch={(value) => {
									dispatch({
										type: 'home/setState',
										payload: {
											openSearchFriend: true
										}
									});
									dispatch({
										type: 'home/searchfriend',
										payload: {
											skipCount: 0,
											maxResultCount: friendPagination.maxResultCount,
											filter: value
										}
									});
								}}
							/>
						</div>
						<div className={styles.toalllist}>
							<Tabs defaultActiveKey="1q">
								<TabPane tab="好友" key="1q">
									{friends.friends ? (
										friends.friends.map((value, index) => {
											return (
												<p
													className={styles.everylist}
													key={index}
													onClick={() => {
														totalk(value);
													}}
												>
													<Badge
														count={value.badge ? value.badge : 0}
														style={{ position: 'absolute', left: -400, top: -20 }}
													/>
													<div
														className={styles.touxiang}
														style={
															value.isOnline ? (
																{}
															) : (
																{
																	WebkitFilter: 'grayscale(100%)',
																	MozFilter: 'grayscale(100%)',
																	MsFilter: 'grayscale(100%)',
																	OFilter: 'grayscale(100%)',
																	filter: 'grayscale(100%)'
																}
															)
														}
													>
														<img
															src={
																remoteUrl +
																'/Profile/GetProfilePictureById/' +
																(value.friendProfilePictureId
																	? value.friendProfilePictureId
																	: '')
															}
															className={styles.touimg}
														/>
													</div>

													<div className={styles.rightlist}>
														<div className={styles.listname}>{value.friendUserName}</div>
													</div>
													<div className={styles.listalltime}>
														<div className={styles.listtime}>
															{value.isOnline ? (
																<span style={{ color: 'red' }}>在线</span>
															) : (
																'离线'
															)}
														</div>
														<div className={styles.listtime2}>
															<span style={{ color: 'red' }}>
																{display ? (
																	''
																) : value.unreadMessageCount !== 0 ? (
																	value.unreadMessageCount + '未读'
																) : (
																	''
																)}
															</span>
														</div>
													</div>
												</p>
											);
										})
									) : (
										''
									)}
								</TabPane>
								<TabPane tab="黑名单" key="2q">
									{friends.blockes ? (
										friends.blockes.map((value, index) => {
											return (
												<p
													className={styles.everylist}
													key={index}
													onClick={() => {
														totalk(value);
													}}
												>
													<div
														className={styles.touxiang}
														style={
															value.isOnline ? (
																{}
															) : (
																{
																	WebkitFilter: 'grayscale(100%)',
																	MozFilter: 'grayscale(100%)',
																	MsFilter: 'grayscale(100%)',
																	OFilter: 'grayscale(100%)',
																	filter: 'grayscale(100%)'
																}
															)
														}
													>
														<img
															src={
																remoteUrl +
																'/Profile/GetProfilePictureById/' +
																(value.friendProfilePictureId
																	? value.friendProfilePictureId
																	: '')
															}
															className={styles.touimg}
														/>
													</div>
													<div className={styles.rightlist}>
														<div className={styles.listname}>{value.friendUserName}</div>
													</div>
													<div className={styles.listalltime}>
														<div className={styles.listtime}>
															{value.isOnline ? (
																<span style={{ color: 'red' }}>在线</span>
															) : (
																'离线'
															)}
														</div>
													</div>
												</p>
											);
										})
									) : (
										''
									)}
								</TabPane>
							</Tabs>
						</div>
					</div>
					<div className={styles.bblist} style={display ? { display: 'block' } : { display: 'none' }}>
						<div className={styles.startbb}>
							<div className={styles.bbname}>
								{sendvalue.friendUserName}
								{sendvalue.isOnline ? (
									<span style={{ color: 'red', marginLeft: 20 }}>· 在线</span>
								) : (
									<span style={{ color: '#ccc', marginLeft: 20 }}>· 离线</span>
								)}
							</div>
							<Popover placement="bottom" content={content} trigger="click">
								<Icon type="ellipsis" style={{ fontSize: 25, color: '#000', marginTop: '20px' }} />
							</Popover>
							<div className={styles.closebb} onClick={closebox}>
								×
							</div>
						</div>
						<div
							className={styles.liaotian}
							ref={(node) => {
								refDom = node;
								refDom ? (refDom.scrollTop = refDom.scrollHeight) : '';
							}}
						>
							{allmessage && allmessage.items ? (
								allmessage.items.map((value, index) => (
									<div className={styles.bigobox}>
										<div className={styles.showtalk}><img
											className={styles.imgbox}
											src={
												remoteUrl +
												(value.side == 1
													? result.user.profile
													: '/Profile/GetProfilePictureById/' +
														(sendvalue.friendProfilePictureId || ''))
											}
											style={value.side == 1 ? { float: 'right' } : { float: 'left' }}
										/>
										<div
											className={styles.talkbox}
											style={value.side == 1 ? { float: 'right' } : { float: 'left' }}
										>
											{value.message}
										</div></div>
										<br />
										<div className={styles.timebox}>{formatMsgTime(value.creationTime)}</div>
									</div>
								))
							) : (
								''
							)}
							{mybbtodo? (
								
								mybbtodo.map((value, index) => (
									value.targetUserId==sendvalue.friendUserId?
									<div className={styles.bigobox}>
										<div className={styles.showtalk}><img
											className={styles.imgbox}
											src={
												remoteUrl +
												(value.side == 1
													? result.user.profile
													: '/Profile/GetProfilePictureById/')
											}
											style={value.side == 1 ? { float: 'right' } : { float: 'left' }}
										/>
										<div
											className={styles.talkbox}
											style={value.side == 1 ? { float: 'right' } : { float: 'left' }}
										>
											{value.message}
										</div></div>
										<br />
										<div className={styles.timebox}>{formatMsgTime(value.creationTime)}</div>
									</div>:''
								))
							) : (
								''
							)}
						</div>
						<div className={styles.sendbb}>
							<div className={styles.biaoqing}>
								<Tooltip placement="top" title={'此功能尚未开通，尽情期待！'}>
									<div className={styles.icons}>
										<Icon type="smile-o" style={{ fontSize: 25 }} />
									</div>
								</Tooltip>
								<Tooltip placement="top" title={'此功能尚未开通，尽情期待！'}>
									<div className={styles.icons}>
										<Icon type="credit-card" style={{ fontSize: 25 }} />
									</div>
								</Tooltip>
							</div>
							<div className={styles.writesend}>
								<FormItem>
									{getFieldDecorator('textshow', {})(
										<TextArea
											rows={3}
											style={{
												resize: 'none',
												border: 'none',
												outline: 'none',
												fontSize: '18px',
												borderColor: 'none',
												boxShadow: 'none'
											}}
											onPressEnter={sendmymessage}
										/>
									)}
								</FormItem>
								<Button
									type="primary"
									style={{ fontWeight: '900', float: 'right', marginRight: '20px' }}
									onClick={sendme}
								>
									发送
								</Button>
								<Button
									type="primary"
									style={{ fontWeight: '900', float: 'right', marginRight: '20px' }}
									onClick={closebox}
								>
									关闭
								</Button>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}

Chat = connect((state) => {
	return {
		...state.home
	};
})(Chat);

export default create()(Chat);
