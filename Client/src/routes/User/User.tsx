import React from 'react';
import AddUser from './modal/AddUser';
import ChangeUser from './modal/ChangeUser';
import ChangePassword from './modal/ChangePassword';
import CustomColumnSelector from './modal/CustomColumnSelector';
import ChangeUserPermission from './modal/ChangeUserPermission';
import PermissionWrapper from './../../components/PermissionWrapper/PermissionWrapper';
import styles from './User.css';
import { Form, Table, Input, Button, Row, Col, Icon, Menu, Dropdown, Badge, Modal, notification } from 'antd';
const create = Form.create;
const FormItem = Form.Item;
const Search = Input.Search;
const confirm = Modal.confirm;

import { connect } from 'dva';

/**
 * User.js
 * Created by 李廷旭 on 2017/9/6 9:30
 * 描述: 用户管理界面
 */
function User({
	dispatch,
	expand,
	form,
	loading,
	pagination,
	addUserModalState,
	changeUserModalState,
	changePasswordModalState,
	items,
	visibleColumnTitles,
	visibleColumnWidth,
	filter,
	selectedUserIds,
	selectedUsers
}) {
	const { getFieldDecorator } = form;
	let form1 = (
		<Row type="flex" align="middle" className={styles.row1}>
			<Col span={10}>
				<Search placeholder="输入关键字搜索，将搜索[用户名][姓名][邮箱][手机号]" onSearch={filterUser} />
			</Col>
			<Col span={6} offset={8} className={styles.tR}>
				<a
					className={styles.collapse}
					onClick={() => {
						dispatch({
							type: 'user/setState',
							payload: {
								expand: !expand,
								filter: {}
							}
						});
					}}
				>
					高级搜索 <Icon type="down" />
				</a>
			</Col>
		</Row>
	);

	let fields = [
		{
			label: '用户名',
			name: 'userName'
		},
		{
			label: '姓名',
			name: 'name'
		},
		{
			label: '手机号',
			name: 'phoneNumber'
		}
	];

	function filterUser(value) {
		dispatch({
			type: 'user/setState',
			payload: {
				filter: { filter: value }
			}
		});
		dispatch({
			type: 'user/getUsers',
			payload: {
				filter: value,
				current: 1,
				pageSize: 10
			}
		});
	}

	function searchUser() {
		form.validateFields((err, values) => {
			let data = {
				name: values.name,
				userName: values.userName,
				phoneNumber: values.phoneNumber
			};
			if (!err) {
				dispatch({
					type: 'user/setState',
					payload: {
						filter: data
					}
				});
				dispatch({
					type: 'user/getUsers',
					payload: {
						...data,
						current: 1,
						pageSize: 10
					}
				});
			}
		});
	}

	function exportUser() {
		form.validateFields((err, values) => {
			let data = {
				name: values.name,
				userName: values.userName,
				phoneNumber: values.phoneNumber,
				filter: expand ? filter.filter : ''
			};
			if (!err) {
				dispatch({
					type: 'user/setState',
					payload: {
						filter: data
					}
				});
				dispatch({
					type: 'user/getUsersToExcel',
					payload: {
						...data
					}
				});
			}
		});
	}

	function resetFilter() {
		form.resetFields();
		dispatch({
			type: 'user/setState',
			payload: {
				filter: {}
			}
		});
	}

	let form2 = (
		<div className={styles.div2}>
			<Form className={styles.form2}>
				<Row gutter={20} className={styles.row2}>
					{(() => {
						const { getFieldDecorator } = form;
						const formCol = {
							labelCol: { span: 8 },
							wrapperCol: { span: 14 }
						};
						return fields.map((ele, index) => {
							return (
								<Col span={8} key={index}>
									<FormItem label={ele.label} {...formCol}>
										{getFieldDecorator(ele.name, {
											initialValue: ele.initialValue
										})(<Input placeholder="请输入" />)}
									</FormItem>
								</Col>
							);
						});
					})()}
				</Row>
				<Row gutter={24} type="flex" align="middle">
					<Col span={16} offset={2}>
						<Button type="primary" onClick={searchUser}>
							查询
						</Button>
						<Button onClick={resetFilter}>清除条件</Button>
					</Col>
					<Col span={6} className={styles.tR}>
						<a
							className={`${styles.collapse} ${styles.collapsePR}`}
							onClick={() => {
								dispatch({
									type: 'user/setState',
									payload: {
										expand: !expand,
										filter: {}
									}
								});
							}}
						>
							收起高级搜索 <Icon type="up" />
						</a>
					</Col>
				</Row>
			</Form>
		</div>
	);

	function handleConfirmPassword(rule, value, callback) {
		if (value && value !== form.getFieldValue('newPasswordUser')) {
			callback('两次输入不一致！');
		}

		// Note: 必须总是返回一个 callback，否则 validateFieldsAndScroll 无法响应
		callback();
	}
	const columns = [
		{
			title: '用户名',
			dataIndex: 'userName',
			sorter: true
		},
		{
			title: '用户编号',
			dataIndex: 'id',
			sorter: true
		},
		{
			title: '姓名',
			dataIndex: 'name',
			sorter: true
		},
		{
			title: '角色',
			dataIndex: 'roles',
			render: (text, record) => {
				let roles = '';
				if (record.roles) {
					record.roles.map((ele, index) => {
						// roles.push(<span key={index}>{ele.roleName}</span>);
						roles += ele.roleName + ' ';
					});
				}
				return roles;
			}
		},
		{
			title: '邮箱地址',
			dataIndex: 'emailAddress',
			sorter: true
		},
		{
			title: '手机号',
			dataIndex: 'phoneNumber',
			sorter: true
		},
		{
			title: '邮箱地址验证',
			dataIndex: 'isEmailConfirmed',
			sorter: true,
			render: (text, record) => {
				if (record.isEmailConfirmed) {
					return <Badge status="success" text="已验证" />;
				} else {
					return <Badge status="default" text="未验证" />;
				}
			}
		},
		{
			title: '手机号码验证',
			dataIndex: 'isPhoneNumberConfirmed',
			sorter: true,
			render: (text, record) => {
				if (record.isPhoneNumberConfirmed) {
					return <Badge status="success" text="已验证" />;
				} else {
					return <Badge status="default" text="未验证" />;
				}
			}
		},
		{
			title: '启用',
			dataIndex: 'isActive',
			sorter: true,
			render: (text, record) => {
				if (record.isActive) {
					return <Badge status="success" text="已启用" />;
				} else {
					return <Badge status="default" text="已禁用" />;
				}
			}
		},
		{
			title: '锁定',
			dataIndex: 'isLocked',
			render: (text, record) => {
				if (record.isLocked) {
					return <Badge status="default" text="已锁定" />;
				} else {
					return <Badge status="success" text="未锁定" />;
				}
			}
		},
		{
			title: '上次登录时间',
			dataIndex: 'lastLoginTime',
			sorter: true
		},
		{
			title: '创建时间',
			dataIndex: 'creationTime',
			sorter: true
		},
		{
			title: '操作',
			dataIndex: 'option',
			width: 80,
			fixed: 'right',
			render: (text, record) => (
				<div>
					{(() => {
						const menu = (
							<PermissionWrapper>
								<Menu>
									<Menu.Item requiredPermission="Pages.Administration.Users.Edit">
										<a
											onClick={() => {
												dispatch({
													type: 'user/getUserForEdit',
													payload: {
														id: record.id
													}
												});
											}}
										>
											编辑
										</a>
									</Menu.Item>
									<Menu.Item requiredPermission="Pages.Administration.Users.ChangePermissions">
										<a
											onClick={() => {
												dispatch({
													type: 'user/setState',
													payload: {
														currentUser: record
													}
												});
												dispatch({
													type: 'user/getUserPermissionsForEdit',
													payload: {
														id: record.id
													}
												});
											}}
										>
											修改权限
										</a>
									</Menu.Item>
									{record.isLocked ? (
										<Menu.Item requiredPermission="Pages.Administration.Users.Unlock">
											<a
												onClick={() => {
													dispatch({
														type: 'user/unlockUser',
														payload: {
															id: record.id,
															name: record.name
														}
													});
												}}
											>
												解锁
											</a>
										</Menu.Item>
									) : null}
									{
										<Menu.Item requiredPermission="Pages.Administration.Users.Edit">
											<a
												onClick={() => {
													confirm({
														title: <div style={{ fontSize: '20px' }}>警告</div>,
														content: (
															<div>
																你确定要重置用户<strong>{record.name}</strong>的登录密码么?
															</div>
														),
														width: 350,
														onOk() {
															dispatch({
																type: 'user/resetUserPassword',
																payload: {
																	id: record.id,
																	name: record.name
																}
															});
														},
														onCancel() {}
													});
												}}
											>
												重置密码
											</a>
										</Menu.Item>
									}
									{
										<Menu.Item requiredPermission="Pages.Administration.Users.Edit">
											<a
												onClick={() => {
													dispatch({
														type: 'user/setState',
														payload: {
															changePasswordModalState: {
																...changePasswordModalState,
																visible: true,
																user: { ...record }
															}
														}
													});
												}}
											>
												修改密码
											</a>
										</Menu.Item>
									}
									{
										<Menu.Item requiredPermission="Pages.Administration.Users.Active">
											<a
												onClick={() => {
													dispatch({
														type: 'user/toggleActiveStatus',
														payload: {
															id: record.id,
															name: record.name,
															activeName: record.isActive ? '禁用' : '启用'
														}
													});
												}}
											>
												{record.isActive ? '禁用' : '启用'}
											</a>
										</Menu.Item>
									}
									<Menu.Item requiredPermission="Pages.Administration.Users.Delete">
										<a
											onClick={() => {
												confirm({
													title: <div style={{ fontSize: '20px' }}>警告</div>,
													content: (
														<div>
															你确定要删除用户<strong>{record.name}</strong>吗?
														</div>
													),
													width: 350,
													onOk() {
														dispatch({
															type: 'user/deleteUser',
															payload: {
																id: record.id,
																name: record.name
															}
														});
													},
													onCancel() {}
												});
											}}
										>
											删除
										</a>
									</Menu.Item>
								</Menu>
							</PermissionWrapper>
						);
						return (
							<PermissionWrapper requiredAnyPermissions="Pages.Administration.Users.Edit,Pages.Administration.Users.Delete">
								<Dropdown overlay={menu}>
									<a className="ant-dropdown-link">
										操作 <Icon type="down" />
									</a>
								</Dropdown>
							</PermissionWrapper>
						);
					})()}
				</div>
			)
		}
	];

	function _onChange(pagination, filters, sorter) {
		dispatch({
			type: 'user/getUsers',
			payload: {
				...pagination,
				...filter,
				...{ sorting: sorter.field ? sorter.field + ' ' + sorter.order.replace('end', '') : '' }
			}
		});
	}

	const rowSelection = {
		onChange: (selectedRowKeys, selectedRows) => {
			dispatch({
				type: 'user/setState',
				payload: {
					selectedUserIds: selectedRowKeys,
					selectedUsers: selectedRows
				}
			});
		}
	};

	function batchDeleteUser() {
		if (!selectedUserIds || !selectedUserIds.length) {
			notification.error({ message: '未选中任何用户!' });
			return;
		}
		confirm({
			title: <div style={{ fontSize: '20px' }}>警告</div>,
			content: (
				<div>
					<div>
						你确定要删除<strong>{selectedUsers[0].name}</strong>等<strong>{selectedUserIds.length}</strong>名用户吗?
					</div>
				</div>
			),
			width: 350,
			onOk() {
				dispatch({
					type: 'user/batchDeleteUser',
					payload: {
						value: selectedUserIds,
						users: selectedUsers
					}
				});
			},
			onCancel() {}
		});
	}
	function batchOperate(operateCn, dispatchType) {
		if (!selectedUserIds || !selectedUserIds.length) {
			notification.error({ message: '未选中任何用户!' });
			return;
		}
		confirm({
			title: <div style={{ fontSize: '20px' }}>警告</div>,
			content: (
				<div>
					<div>
						你确定要{operateCn}
						<strong>{selectedUsers[0].name}</strong>等<strong>{selectedUserIds.length}</strong>名用户吗?
					</div>
				</div>
			),
			width: 350,
			onOk() {
				dispatch({
					type: dispatchType,
					payload: {
						value: selectedUserIds,
						users: selectedUsers
					}
				});
			},
			onCancel() {}
		});
	}

	const dropdownMenu = (
		<PermissionWrapper>
			<Menu>
				<Menu.Item key="1" onClick={batchDeleteUser} requiredPermission="Pages.Administration.Users.Delete">
					<a onClick={batchDeleteUser}>批量删除</a>
				</Menu.Item>
				<Menu.Item key="2" requiredPermission="Pages.Administration.Users.Unlock">
					<a onClick={() => batchOperate('解锁', 'user/batchUnlockUser')}>批量解锁</a>
				</Menu.Item>
				<Menu.Item key="3" requiredPermission="Pages.Administration.Users.Active">
					<a onClick={() => batchOperate('启用', 'user/batchActiveUser')}>批量启用</a>
				</Menu.Item>
				<Menu.Item key="4" requiredPermission="Pages.Administration.Users.Active">
					<a onClick={() => batchOperate('禁用', 'user/batchNotActiveUser')}>批量禁用</a>
				</Menu.Item>
			</Menu>
		</PermissionWrapper>
	);

	return (
		<div>
			<div className={styles.normalT}>{expand ? form2 : form1}</div>
			<div className={styles.normalB}>
				<Row type="flex" align="middle" className={styles.btns}>
					<Col span={16}>
						<PermissionWrapper requiredPermission="Pages.Administration.Users.Create">
							<Button
								type="primary"
								onClick={() => {
									dispatch({
										type: 'user/setState',
										payload: {
											addUserModalState: { visible: true, activeTabKey: '1' },
											organizationFilter: '',
											selectedOrganizations: []
										}
									});
								}}
							>
								添加用户
							</Button>
						</PermissionWrapper>
						<Button onClick={exportUser}>导出到EXCEL</Button>
						<Dropdown overlay={dropdownMenu}>
							<Button style={{ marginLeft: 8 }}>
								更多操作<Icon type="down" />
							</Button>
						</Dropdown>
					</Col>
					<Col span={8} className={styles.tR}>
						<CustomColumnSelector />
					</Col>
				</Row>
				<Table
					columns={columns.filter((c) => visibleColumnTitles.indexOf(c.title) >= 0)}
					dataSource={items}
					pagination={pagination}
					loading={loading}
					onChange={_onChange}
					rowSelection={rowSelection}
					rowKey={(record) => (record.id ? record.id : record.key)}
					bordered={true}
					size="middle"
					scroll={{ x: visibleColumnWidth }}
				/>
			</div>
			{addUserModalState.visible ? <AddUser /> : null}
			{changeUserModalState.visible ? <ChangeUser /> : null}

			<ChangeUserPermission />
			<ChangePassword />
		</div>
	);
}

User = connect((state) => {
	return {
		...state.user,
		loading: state.loading.effects['user/getUsers']
	};
})(User);

export default create()(User);
