import React from 'react';
import styles from './Notification.css';
import NotificationInfo from './modal/NotificationInfo';
import {
	Form,
	Table,
	Input,
	Button,
	Row,
	Col,
	Icon,
	Menu,
	Dropdown,
	DatePicker,
	Select,
	InputNumber,
	Badge
} from 'antd';
const create = Form.create;
const FormItem = Form.Item;
const Search = Input.Search;
const InputGroup = Input.Group;
const RangePicker = DatePicker.RangePicker;
const Option = Select.Option;
import moment from 'moment';

import { connect } from 'dva';

function Notification({ dispatch, expand, form, loading, pagination, visible, items }) {
	const { getFieldDecorator } = form;

	const columns = [
		{
			title: '阅读状态',
			dataIndex: 'state',
			render: (text, record) => <span>{record.state == 0 ? '未读' : '已读'}</span>
		},
		{
			title: '时间',
			dataIndex: 'notification.creationTime',
			render: (text, record) => (
				<span>
					{record.notification ? moment(record.notification.creationTime).format('YYYY-MM-DD HH:mm') : ''}
				</span>
			)
		},
		{
			title: '发送人',
			render: (text, record) => <span>系统发送</span>
		},
		{
			title: '标题',
			render: (text, record) =>
				record.notification && record.notification.data && record.notification.data.properties.message ? (
					<span>{record.notification.data.properties.message}</span>
				) : (
					<span>系统通知</span>
				)
		},
		{
			title: '内容',
			render: (text, record) =>
				record.notification && record.notification.data && record.notification.data.properties.content ? (
					<span>{record.notification.data.properties.content}</span>
				) : record.notification && record.notification.data && record.notification.data.properties.message ? (
					<span>{record.notification.data.properties.message}</span>
				) : (
					''
				)
		},
		{
			title: '操作',
			dataIndex: 'option',
			width: 160,
			fixed: 'right',
			render: (text, record) => (
				<span>
					<a
						type="primary"
						onClick={() => {
							dispatch({
								type: 'notification/setState',
								payload: {
									visible: !visible,
									record: record
								}
							});
							if (record.state == 0) {
								dispatch({
									type: 'notification/setNotificationAsRead',
									payload: {
										id: record.id
									}
								});
							}
						}}
					>
						查看详情
					</a>
					<span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
					{record.state == 0 ? (
						<a
							type="primary"
							onClick={() => {
								dispatch({
									type: 'notification/setNotificationAsRead',
									payload: {
										id: record.id
									}
								});
							}}
						>
							标记为已读
						</a>
					) : (
						''
					)}
				</span>
			)
		}
	];

	function _onChange(pagination, filters, sorter) {
		dispatch({
			type: 'notification/getUserNotifications',
			payload: {
				...pagination,
				...sorter
			}
		});
	}

	// rowSelection object indicates the need for row selection
	const rowSelection = {
		onChange: (selectedRowKeys, selectedRows) => {
			console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
		}
	};

	let from1 = (
		<div>
			<Form layout="inline" className="ant-advanced-search-form">
				<Row gutter={16}>
					<FormItem label="是否已读">
						{getFieldDecorator('state', {
							initialValue: ''
						})(
							<Select style={{ width: '100px' }}>
								<Option value="">全部</Option>
								<Option value="0">未读</Option>
								<Option value="1">已读</Option>
							</Select>
						)}
					</FormItem>
					<Button
						type="primary"
						onClick={() => {
							form.validateFields((err, values) => {
								if (!err) {
									var payload = {
										...pagination,
										...values
									};
									console.log(payload);
									dispatch({
										type: 'notification/getUserNotifications',
										payload: payload
									});
								}
							});
						}}
					>
						查询
					</Button>
				</Row>
			</Form>
		</div>
	);

	return (
		<div>
			<div className={styles.normalT}>
				<div>{from1}</div>
			</div>
			<div className={styles.normalB}>
				<div className={styles.btns} />
				<Table
					columns={columns}
					dataSource={items}
					pagination={pagination}
					loading={loading}
					onChange={_onChange}
					rowKey={(record) => record.id}
					bordered={true}
					size="middle"
					scroll={{ x: 1300 }}
				/>
			</div>
			<NotificationInfo />
		</div>
	);
}

Notification = connect((state) => {
	return {
		...state.notification,
		loading: state.loading.effects['notification/getUserNotifications']
	};
})(Notification);

export default create()(Notification);
