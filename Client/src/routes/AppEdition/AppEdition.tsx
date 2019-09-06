import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Link } from 'dva/router';
import styles from './AppEdition.css';
import { Table, Button, Input, Modal, Checkbox, Tag, Row, Col, Badge } from 'antd';
const { Column, ColumnGroup } = Table;
const Search = Input.Search;
const confirm = Modal.confirm;
import { remoteUrl } from '../../utils/url';
import AppEditionModal from './Modal/AppEditionModal';

function AppEdition({ dispatch, items, loading, pagination, modalVisible }) {
	function showAddModal(type) {
		dispatch({
			type: 'appEdition/setState',
			payload: {
				modalVisible: true,
				modalText: '添加' + type + '版本',
				isIOS: type == 'IOS',
				isAdd: true,
				record: {}
			}
		});
	}

	function showEditModal(record) {
		dispatch({
			type: 'appEdition/setState',
			payload: {
				modalVisible: true,
				record: record,
				modalText: '编辑' + record.appType + '版本',
				isIOS: record.appType == 'IOS',
				isAdd: false
			}
		});
	}

	function showConfirm(id, record) {
		confirm({
			title: <div style={{ fontSize: '20px' }}>警告</div>,
			content: (
				<div>
					<div>你确定要删除此版本吗?</div>
				</div>
			),
			width: 350,
			onOk() {
				dispatch({
					type: 'appEdition/deleteAppEdition',
					payload: {
						id: id
					}
				});
			},
			onCancel() { }
		});
	}

	function _onChange(pagination, filters, sorter) {
		dispatch({
			type: 'appEdition/getAppEditions',
			payload: {
				...pagination,
				...sorter
			}
		});
	}

	return (
		<div>
			<Row type="flex" align="middle" className={styles.row1}>
				<Col span={8}>
					<Search
						placeholder="输入版本号搜索"
						size="large"
						onSearch={(value) =>
							dispatch({
								type: 'appEdition/getAppEditions',
								payload: {
									filter: value
								}
							})}
					/>
				</Col>
			</Row>
			<div className={styles.tabbox}>
				<Button type="primary" className={styles.adds} onClick={showAddModal.bind(this, 'IOS')}>
					添加IOS版本
				</Button>
				<Button type="primary" className={styles.adds} onClick={showAddModal.bind(this, 'Android')}>
					添加Android版本
				</Button>
				<Table
					dataSource={items}
					bordered={true}
					size="middle"
					rowKey={(record) => record.id}
					pagination={pagination}
					loading={loading}
					onChange={_onChange}
				>
					<Column title="系统" dataIndex="appType" sorter />
					<Column title="版本号" dataIndex="version" sorter />
					<Column
						title="关于"
						dataIndex="aboutUrl"
						render={(text, record) => {
							return (
								<a target="_blank" href={record.aboutUrl}>
									关于
								</a>
							);
						}}
					/>
					<Column
						title="是否强制更新"
						sorter
						dataIndex="isMandatoryUpdate"
						render={(text, record) => {
							return (record.isMandatoryUpdate ?
								<Badge status="success" text="是" />
								:
								<Badge status="default" text="否" />
							);
						}}
					/>
					<Column
						title="是否启用"
						sorter
						dataIndex="isActive"
						render={(text, record) => {
							return (
								record.isActive ?
									<Badge status="success" text="是" /> : <Badge status="default" text="否" />)
						}}
					/>
					<Column
						title="描述"
						dataIndex="describe"
						render={(text, record) => {
							return <div className={styles.tableText}>{record.describe}</div>
						}}
					/>
					<Column
						title="Itunes连接"
						dataIndex="itunesUrl"
						render={(text, record) => {
							return record.appType == 'IOS' ? (
								<a target="_blank" href={record.itunesUrl} disabled={!Boolean(record.itunesUrl)}>
									Itunes连接
								</a>
							) : null;
						}}
					/>
					<Column title="创建时间" dataIndex="creationTime" sorter />
					<Column
						title="操作"
						key="action"
						render={(text, record) => (
							<span>
								<a
									disabled={!Boolean(record.installationPackage)}
									target="_blank"
									href={
										remoteUrl + '/api/services/app/appEditions/DownloadAppEdition?id=' + record.id
									}
								>
									下载
								</a>
								<span style={{ marginLeft: '6px' }} />
								<a onClick={showEditModal.bind(this, record)}>编辑</a>
								<span style={{ marginLeft: '6px' }} />
								<a onClick={showConfirm.bind(this, record.id, record)}>删除</a>
							</span>
						)}
					/>
				</Table>
			</div>
			{modalVisible ? <AppEditionModal /> : null}
		</div>
	);
}
AppEdition = connect((state) => {
	return {
		...state.appEdition,
		loading: state.loading.effects['appEdition/getAppEditions']
	};
})(AppEdition);

export default AppEdition;
