import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Input, InputNumber, Badge, DatePicker, Menu, Modal, Dropdown, Icon, Button } from 'antd';
const confirm = Modal.confirm;

const create = Form.create;
const FormItem = Form.Item;

const { TextArea } = Input;
import * as api from '../../api/api';

import CRUD from '../CRUD/CRUD';
import moment from 'moment';
import PermissionWrapper from './../../components/PermissionWrapper/PermissionWrapper';
import AddUser from '../User/modal/AddUser';
import ChangeUser from '../User/modal/ChangeUser';
import ChangePassword from '../User/modal/ChangePassword';
import CustomColumnSelector from '../User/modal/CustomColumnSelector';
import ChangeUserPermission from '../User/modal/ChangeUserPermission';

function User({
	form,
	record,
	addUserModalState,
	changeUserModalState,
	changePasswordModalState,
	selectedUserIds,
	selectedUsers
}) {
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
	const { getFieldDecorator } = form;
	const formCol = {
		labelCol: { span: 8 },
		wrapperCol: { span: 12 }
	};

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
			<CRUD
				form={form}
				getAllApi={new api.UserApi().appUserGetUsers}
				toExcelApi={new api.UserApi().appUserGetUsersToExcel}
				columns={columns}
				filterProps={{
					filters: [
						{
							displayName: '输入关键字搜索，将搜索[用户名][姓名][邮箱][手机号]'
						}
					],
					advancedFilters: [
						{
							displayName: '用户名',
							name: 'userName'
						},
						{
							displayName: '姓名',
							name: 'name'
						},
						{
							displayName: '手机号',
							name: 'phoneNumber'
						}
					]
				}}
				tableProps={{ rowSelection: rowSelection }}
				perToolButtons={
					<PermissionWrapper requiredPermission="Pages.Administration.Users.Create">
						<Button
							style={{ marginRight: '12px' }}
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
				}
				postToolButtons={
					<Dropdown overlay={dropdownMenu}>
						<Button>
							更多操作<Icon type="down" />
						</Button>
					</Dropdown>
				}
			/>

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
		...state.crud
	};
})(User);
export default create()(User);
