import React from 'react';
import styles from './../OrganizationList.css';
import { Modal, Input, Button, Row, Col, Icon, Select, Radio, Table } from 'antd';
import { connect } from 'dva';

const Search = Input.Search;

/**
 * AddOrganizationUserModal.js
 * Created by 凡尧 on 2017/9/13 14:47
 * 描述: 添加组织机构成员弹窗
 */
function AddOrganizationUserModal({
	dispatch,
	addOrganizationUserModalVisible,
	organization,
	validUserList,
	selectedUsers,
	validUserPagination,
	validUserLoading,
	validUserNameFilter
}) {
	function handleCancel() {
		dispatch({
			type: 'organization/setState',
			payload: {
				addOrganizationUserModalVisible: !addOrganizationUserModalVisible
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
			dataIndex: 'creationTime'
		}
	];

	const rowSelection = {
		selectedRowKeys: selectedUsers,
		onChange: (selectedRowKeys, selectedRows) => {
			dispatch({
				type: 'organization/setState',
				payload: {
					selectedUsers: selectedRowKeys
				}
			});
		},
		selections: [ { key: 'id' } ]
	};

	function handleTableChange(pagination, filters, sorter) {
		dispatch({
			type: 'organization/getOrganizationUnitJoinableUserList',
			payload: {
				id: organization.id,
				skipCount: pagination.pageSize * (pagination.current - 1),
				maxResultCount: pagination.pageSize,
				filter: validUserNameFilter
			}
		});
	}

	function onSearch(value) {
		dispatch({
			type: 'organization/setState',
			payload: {
				validUserNameFilter: value
			}
		});
		dispatch({
			type: 'organization/getOrganizationUnitJoinableUserList',
			payload: {
				id: organization.id,
				skipCount: 0,
				maxResultCount: 5,
				filter: value
			}
		});
	}

	return (
		<Modal
			visible={addOrganizationUserModalVisible}
			width={658}
			title="添加成员"
			onCancel={handleCancel}
			footer={[
				<Button
					key="back"
					type="primary"
					onClick={() => {
						var userIdListStr = '';
						selectedUsers.map((u) => {
							userIdListStr = userIdListStr + u + ',';
						});
						dispatch({
							type: 'organization/addUserToOrganizationUnit',
							payload: {
								userIdListStr: userIdListStr,
								organizationUnitId: organization.id
							}
						});
					}}
				>
					保存
				</Button>,
				<Button key="submit" onClick={handleCancel}>
					取消
				</Button>
			]}
		>
			<Search onSearch={onSearch} placeholder="输入关键字搜索" className={styles.search} />

			<Table
				columns={columns}
				rowSelection={rowSelection}
				pagination={validUserPagination}
				rowKey={(user) => user.id}
				dataSource={validUserList.items}
				onChange={handleTableChange}
				loading={validUserLoading}
			/>
		</Modal>
	);
}

AddOrganizationUserModal = connect((state) => {
	return {
		...state.organization
	};
})(AddOrganizationUserModal);

export default AddOrganizationUserModal;
