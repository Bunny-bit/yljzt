import React from 'react';
import { Modal, Tree, Button, Input, Icon, Row, Col, message, Dropdown, Menu, Table } from 'antd';
import { connect } from 'dva';
import EditOrganizationModal from './modal/EditOrganizationModal';
import AddOrganizationUserModal from './modal/AddOrganizationUserModal';
import PermissionWrapper from './../../components/PermissionWrapper/PermissionWrapper';

import styles from './OrganizationList.css';
const TreeNode = Tree.TreeNode;
const confirm = Modal.confirm;

/**
 * OrganizationList.js
 * Created by 凡尧 on 2017/9/13 12:00
 * 描述: 组织机构管理界面
 */
function OrganizationList({
	dispatch,
	expand,
	organizations,
	editOrganizationModalVisible,
	userList,
	userPagination,
	userLoading,
	organization,
	addOrganizationUserModalVisible
}) {
	function onSelect(expandedKeys, info) {
		if (info.selectedNodes.length != 1) {
			return;
		}
		dispatch({
			type: 'organization/setState',
			payload: {
				organization: findNode(organizations, info.selectedNodes[0].key)
			}
		});
		dispatch({
			type: 'organization/getOrganizationUnitUsers',
			payload: {
				id: Number(info.selectedNodes[0].key),
				maxResultCount: 5,
				skipCount: 0,
				isRecursiveSearch: false
			}
		});
	}

	function findDropNode(root, id) {
		for (let i = 0; i < root.length; i++) {
			if (root[i].id == id) {
				let node = {
					newParentId: root[i].parentId
				};
				return node;
			} else {
				if (root[i].children && root[i].children.length > 0) {
					var node = findDropNode(root[i].children, id);
					if (node) {
						return node;
					}
				}
			}
		}
	}

	function onDrop(info) {
		const dropKey = info.node.props.eventKey;
		const dragKey = info.dragNode.props.eventKey;
		const dropPos = info.node.props.pos.split('-');
		const dropPosition = info.dropPosition - Number(dropPos[dropPos.length - 1]);
		if (info.dropToGap) {
			var node = findDropNode(organizations, dropKey);
			dispatch({
				type: 'organization/moveOrganizationUnit',
				payload: {
					id: dragKey,
					newParentId: node.newParentId
				}
			});
		} else {
			dispatch({
				type: 'organization/moveOrganizationUnit',
				payload: {
					id: dragKey,
					newParentId: dropKey
				}
			});
		}
	}

	function findNode(root, id) {
		if (!root) {
			return;
		}
		for (var i in root) {
			if (root[i].id == id) {
				const node = { parent: 0, ...root[i] };
				return node;
			}
			var node = findNode(root[i].children, id);
			if (node) {
				if (node.parent == 0) {
					node.parent = root[i].id;
				}
				return node;
			}
		}
	}

	function confirmDeleteOrganization(item) {
		confirm({
			title: <div style={{ fontSize: '20px' }}>警告</div>,
			content: <div>{'你确定要删除' + item.displayName + '吗？'}</div>,
			width: 350,
			onOk() {
				dispatch({
					type: 'organization/deleteOrganizationUnit',
					payload: {
						id: item.id
					}
				});
			},
			onCancel() {}
		});
	}

	function confirmRemoveOrganizationMember(member) {
		confirm({
			title: <div style={{ fontSize: '20px' }}>警告</div>,
			content: <div>{'你确定要从' + organization.displayName + '移除' + member.userName + '吗？'} </div>,
			width: 350,
			onOk() {
				removeUser(member.id);
			},
			onCancel() {}
		});
	}

	const buildTitle = (item) => {
		const handlerList = (
			<Menu>
				<Menu.Item>
					<a
						onClick={() => {
							dispatch({
								type: 'organization/setState',
								payload: {
									isAddOrganization: true,
									editOrganizationModalVisible: !editOrganizationModalVisible,
									parentId: item.id
								}
							});
						}}
					>
						添加子级
					</a>
				</Menu.Item>
				<Menu.Item>
					<a
						onClick={() => {
							dispatch({
								type: 'organization/setState',
								payload: {
									isAddOrganization: false,
									editOrganizationModalVisible: !editOrganizationModalVisible,
									organization: item,
									editOrganizationId: organization.id
								}
							});
						}}
					>
						修改
					</a>
				</Menu.Item>
				<Menu.Item>
					<a onClick={confirmDeleteOrganization.bind(this, item)}>删除</a>
				</Menu.Item>
			</Menu>
		);

		return (
			<div>
				<div style={{ float: 'left', marginRight: '30px' }}>
					{item.displayName} ({item.memberCount})
				</div>
				<PermissionWrapper requiredPermission="Pages.Administration.OrganizationUnits.ManageOrganizationTree">
					<Dropdown overlay={handlerList} trigger={[ 'click' ]}>
						<a className="ant-dropdown-link">
							操作
							<Icon type="down" />
						</a>
					</Dropdown>
				</PermissionWrapper>
			</div>
		);
	};

	const loopOrganizations = (data) =>
		data.map((item) => {
			if (item.children && item.children.length) {
				return (
					<TreeNode key={item.id} title={buildTitle(item)}>
						{loopOrganizations(item.children)}
					</TreeNode>
				);
			}
			return <TreeNode key={item.id} title={buildTitle(item)} />;
		});

	function removeUser(id) {
		dispatch({
			type: 'organization/removeUserFromOrganizationUnit',
			payload: {
				userIdListStr: id,
				organizationUnitId: organization.id
			}
		});
	}

	const columns = [
		{
			title: '用户名',
			dataIndex: 'userName'
		},
		{
			title: '姓名',
			dataIndex: 'name'
		},
		{
			title: '手机号',
			dataIndex: 'phoneNumber'
		},
		{
			//   title: '邮箱',
			//   dataIndex: 'emailAddress',
			// }, {
			title: '添加时间',
			dataIndex: 'addedTime'
		},
		{
			title: '操作',
			dataIndex: '',
			key: 'x',
			render: (text, record, index) => (
				<span>
					<PermissionWrapper requiredPermission="Pages.Administration.OrganizationUnits.ManageMembers">
						<a onClick={confirmRemoveOrganizationMember.bind(this, record)}>移除</a>
					</PermissionWrapper>
				</span>
			)
		}
	];

	function handleTableChange(pagination, filters, sorter) {
		dispatch({
			type: 'organization/getOrganizationUnitUsers',
			payload: {
				id: organization.id,
				skipCount: pagination.pageSize * (pagination.current - 1),
				maxResultCount: pagination.pageSize,
				isRecursiveSearch: false
			}
		});
	}

	return (
		<div>
			<Row>
				<Col span={11} className={styles.normalC}>
					<Row className={styles.titlehead}>
						<Col span={12} className={styles.norhead}>
							组织机构
						</Col>
						<Col span={12} className={styles.noradd}>
							<PermissionWrapper requiredPermission="Pages.Administration.OrganizationUnits.ManageOrganizationTree">
								<Button
									type="primary"
									className={styles.norbut}
									onClick={() => {
										dispatch({
											type: 'organization/setState',
											payload: {
												isAddOrganization: true,
												editOrganizationModalVisible: !editOrganizationModalVisible,
												parentId: 0
											}
										});
									}}
								>
									添加根组织机构
								</Button>
							</PermissionWrapper>
						</Col>
					</Row>
					<Row>
						<Col span={24}>
							<Tree showLine className="draggable-tree" draggable onDrop={onDrop} onSelect={onSelect}>
								{loopOrganizations(organizations)}
							</Tree>
						</Col>
					</Row>
				</Col>
				<Col span={11} className={styles.normalD}>
					<Row className={styles.titlehead}>
						<Col span={12} className={styles.norhead}>
							{organization && organization.displayName ? organization.displayName : '组织成员'}
						</Col>
						<Col span={12} className={styles.noradd}>
							<PermissionWrapper requiredPermission="Pages.Administration.OrganizationUnits.ManageMembers">
								<Button
									type="primary"
									disabled={!organization || !organization.id}
									onClick={() => {
										dispatch({
											type: 'organization/getOrganizationUnitJoinableUserList',
											payload: {
												id: organization.id,
												skipCount: 0,
												maxResultCount: 5,
												nameFilter: ''
											}
										});
										dispatch({
											type: 'organization/setState',
											payload: {
												addOrganizationUserModalVisible: !addOrganizationUserModalVisible
											}
										});
									}}
								>
									添加成员
								</Button>
							</PermissionWrapper>
						</Col>
					</Row>
					<Row>
						<Col>
							<Table
								columns={columns}
								pagination={userPagination}
								loading={userLoading}
								rowKey={(record) => record.id}
								dataSource={userList.items}
								onChange={handleTableChange}
							/>
						</Col>
					</Row>
				</Col>
			</Row>

			{editOrganizationModalVisible ? <EditOrganizationModal /> : null}

			<AddOrganizationUserModal />
		</div>
	);
}
OrganizationList = connect((state) => {
	return {
		...state.organization
	};
})(OrganizationList);

export default OrganizationList;
