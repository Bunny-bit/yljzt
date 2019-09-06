import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Link } from 'dva/router';
import styles from './Role.css';
import { Table, Button, Input, Modal, Tabs, Checkbox, Tree, Tag, Row, Col } from 'antd';
import AddRoleModal from './modal/AddRoleModal';
import EditRoleModal from './modal/EditRoleModal';
import PermissionWrapper from './../../components/PermissionWrapper/PermissionWrapper';
const { Column, ColumnGroup } = Table;
const Search = Input.Search;
const confirm = Modal.confirm;
const TabPane = Tabs.TabPane;
const TreeNode = Tree.TreeNode;

function Role({ dispatch, roles, addRoleModalState, editRoleModalState, editingRole, permissionTree }) {
	function showModal() {
		dispatch({
			type: 'role/setState',
			payload: {
				addRoleModalState: { visible: true, activeTabKey: '1' },
				selectRoles: []
			}
		});
	}

	function showModal2(id) {
		dispatch({
			type: 'role/getRoleForEdit',
			payload: {
				id: id
			}
		});
	}

	function showConfirm(id, record) {
		confirm({
			title: <div style={{ fontSize: '20px' }}>警告</div>,
			content: (
				<div>
					<div>
						你确定要删除此角色<strong>{record.displayName}</strong>吗?
					</div>
					<div>拥有此角色的用户将移除此角色</div>
				</div>
			),
			width: 350,
			onOk() {
				dispatch({
					type: 'role/deleteRole',
					payload: {
						id: id,
						displayName: record.displayName
					}
				});
			},
			onCancel() {}
		});
	}

	const loopPermissions = (data) =>
		data.map((item) => {
			if (item.children && item.children.length) {
				return (
					<TreeNode key={item.name} title={item.displayName}>
						{loopPermissions(item.children)}
					</TreeNode>
				);
			}
			return <TreeNode key={item.name} title={item.displayName} />;
		});

	return (
		<div>
			<Row type="flex" align="middle" className={styles.row1}>
				<Col span={8}>
					<Search
						placeholder="输入关键字搜索"
						onSearch={(value) =>
							dispatch({
								type: 'role/getRoles',
								payload: {
									filter: value
								}
							})}
					/>
				</Col>
			</Row>
			<div className={styles.tabbox}>
				<PermissionWrapper requiredPermission="Pages.Administration.Roles.Create">
					<Button type="primary" className={styles.adds} onClick={showModal}>
						添加角色
					</Button>
				</PermissionWrapper>
				<Table
					dataSource={roles}
					bordered={true}
					size="middle"
					rowKey={(record) => record.id}
					pagination={false}
				>
					<Column title="角色名称" dataIndex="displayName" key="displayName" />
					<Column
						title="角色标签"
						key="label"
						render={(text, record) => {
							if (record.isStatic && record.isDefault) {
								return (
									<div>
										<Tag color="blue">系统</Tag>
										<Tag color="blue">默认</Tag>
									</div>
								);
							} else if (record.isStatic) {
								return (
									<div>
										<Tag color="blue">系统</Tag>
									</div>
								);
							} else if (record.isDefault) {
								return (
									<div>
										<Tag color="blue">默认</Tag>
									</div>
								);
							}
							return <span />;
						}}
					/>
					<Column title="创建时间" dataIndex="creationTime" key="creationTime" />
					<Column
						title="操作"
						key="action"
						render={(text, record) => (
							<PermissionWrapper requiredAnyPermissions="Pages.Administration.Roles.Edit,Pages.Administration.Roles.Delete">
								<span>
									<PermissionWrapper requiredPermission="Pages.Administration.Roles.Edit">
										<a onClick={showModal2.bind(this, record.id)}>编辑</a>
									</PermissionWrapper>
									<span style={{ marginLeft: '6px' }} />
									<PermissionWrapper requiredPermission="Pages.Administration.Roles.Delete">
										<a onClick={showConfirm.bind(this, record.id, record)}>删除</a>
									</PermissionWrapper>
								</span>
							</PermissionWrapper>
						)}
					/>
				</Table>
				{addRoleModalState.visible ? <AddRoleModal /> : null}
				{editRoleModalState.visible ? <EditRoleModal /> : null}
			</div>
		</div>
	);
}
Role = connect((state) => {
	return {
		...state.role
	};
})(Role);

export default Role;
