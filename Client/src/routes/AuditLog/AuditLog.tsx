import React from 'react';
import styles from './AuditLog.css';
import AuditLogInfo from './modal/AuditLogInfo';
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
import PermissionWrapper from './../../components/PermissionWrapper/PermissionWrapper';

import { connect } from 'dva';
/**
 * AuditLog.js
 * Created by 刘宏玺 on 2017/9/7 15:00
 * 描述: 审计日志
 */
function AuditLog({ dispatch, expand, form, loading, pagination, visible, items }) {
	const { getFieldDecorator } = form;

	const columns = [
		{
			title: '状态',
			dataIndex: 'exception',
			render: (text, record) => (
				<center>
					{record.exception ? <Badge status="default" text="错误" /> : <Badge status="success" text="成功" />}
				</center>
			)
		},
		{
			title: '时间',
			dataIndex: 'executionTime',
			sorter: true
		},
		{
			title: '用户名',
			dataIndex: 'userName',
			sorter: true
		},
		{
			title: 'Service',
			dataIndex: 'serviceName',
			sorter: true
		},
		{
			title: 'Action',
			dataIndex: 'methodName',
			sorter: true
		},
		{
			title: '相应时长',
			dataIndex: 'executionDuration',
			sorter: true,
			render: (text, record) => `${text}ms`
		},
		{
			title: 'IP',
			dataIndex: 'clientIpAddress',
			sorter: true
		},
		{
			title: '客户端',
			dataIndex: 'clientName',
			sorter: true
		},
		{
			title: '浏览器',
			dataIndex: 'browserInfo',
			sorter: true
		},
		{
			title: '操作',
			dataIndex: 'option',
			width: 80,
			fixed: 'right',
			render: (text, record) => (
				<center>
					<a
						type="primary"
						onClick={() => {
							dispatch({
								type: 'auditLog/setState',
								payload: {
									visible: !visible,
									record: record
								}
							});
						}}
					>
						查看详情
					</a>
				</center>
			)
		}
	];

	function _onChange(pagination, filters, sorter) {
		dispatch({
			type: 'auditLog/getAuditLogs',
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
					<FormItem label="日期范围">
						{getFieldDecorator('dateRange', {
							initialValue: [ moment().subtract(3, 'days'), moment() ]
						})(<RangePicker style={{ width: '100%' }} format="YYYY-MM-DD" />)}
					</FormItem>
					<PermissionWrapper>
						<Button
							type="primary"
							permisions={''}
							visible={false}
							onClick={() => {
								form.validateFields((err, values) => {
									if (!err) {
										var payload = {
											...pagination,
											...values,
											...{ isAdvancedSearch: false }
										};
										dispatch({
											type: 'auditLog/getAuditLogs',
											payload: payload
										});
									}
								});
							}}
						>
							查询
						</Button>
					</PermissionWrapper>
					<a
						className={styles.collapseRight}
						onClick={() => {
							console.log(expand);
							dispatch({
								type: 'auditLog/setState',
								payload: {
									expand: !expand
								}
							});
						}}
					>
						<span>
							高级搜索 <Icon type="down" />
						</span>
					</a>
				</Row>
			</Form>
		</div>
	);

	let from2 = (
		<div className={styles.form2}>
			<Form layout="horizontal" className="ant-advanced-search-form">
				<Row type="flex" justify="space-around" align="middle">
					<Col span={12}>
						<FormItem label="日期范围" labelCol={{ span: 3 }} wrapperCol={{ span: 21 }}>
							{getFieldDecorator('dateRange1', {
								initialValue: [ moment().subtract(3, 'days'), moment() ]
							})(<RangePicker style={{ width: '100%' }} format="YYYY-MM-DD" />)}
						</FormItem>
					</Col>
					<Col span={6}>
						<FormItem label="用户名" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
							{getFieldDecorator('userName')(<Input />)}
						</FormItem>
					</Col>
					<Col span={6}>
						<FormItem label="相应时长" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
							{getFieldDecorator('minExecutionDuration')(<InputNumber style={{ width: '35%' }} />)}
							<span style={{ marginRight: 8 }}>-</span>
							{getFieldDecorator('maxExecutionDuration')(<InputNumber style={{ width: '35%' }} />)}
						</FormItem>
					</Col>
					<Col span={6}>
						<FormItem label="Service" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
							{getFieldDecorator('serviceName')(<Input />)}
						</FormItem>
					</Col>
					<Col span={6}>
						<FormItem label="Action" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
							{getFieldDecorator('methodName')(<Input />)}
						</FormItem>
					</Col>
					<Col span={6}>
						<FormItem label="浏览器" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
							{getFieldDecorator('browserInfo')(<Input />)}
						</FormItem>
					</Col>
					<Col span={6}>
						<FormItem label="错误状态" labelCol={{ span: 6 }} wrapperCol={{ span: 18 }}>
							{getFieldDecorator('hasException', {
								initialValue: ''
							})(
								<Select style={{ width: '100%' }}>
									<Option value="">全部</Option>
									<Option value="false">成功</Option>
									<Option value="true">出现错误</Option>
								</Select>
							)}
						</FormItem>
					</Col>
				</Row>
				<Row type="flex" justify="space-around" align="middle">
					<Col span={24} offset={3}>
						<Button
							type="primary"
							onClick={() => {
								form.validateFields((err, values) => {
									if (!err) {
										var payload = {
											...pagination,
											...values,
											...{ isAdvancedSearch: true }
										};
										dispatch({
											type: 'auditLog/getAuditLogs',
											payload: payload
										});
									}
								});
							}}
						>
							查询
						</Button>

						<Button
							onClick={() => {
								form.resetFields();
							}}
						>
							清除条件
						</Button>
					</Col>
				</Row>
			</Form>

			<a
				className={styles.collapseRight}
				style={{ bottom: 22 }}
				onClick={() => {
					console.log(expand);

					form.resetFields();
					dispatch({
						type: 'auditLog/setState',
						payload: {
							expand: !expand
						}
					});
				}}
			>
				<span>
					收起高级搜索 <Icon type="up" />
				</span>
			</a>
		</div>
	);

	return (
		<div>
			<div className={styles.normalT}>
				<div>{expand ? from2 : from1}</div>
			</div>
			{/*<div className={styles.normalB}>*/}
			<div className={styles.btns}>
				<Button
					onClick={() => {
						form.validateFields((err, values) => {
							if (!err) {
								var payload = {
									...pagination,
									...values,
									...{ isAdvancedSearch: visible }
								};
								dispatch({
									type: 'auditLog/getAuditLogsToExcel',
									payload: payload
								});
							}
						});
					}}
				>
					导出到EXCEL
				</Button>
			</div>
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
			<AuditLogInfo />
		</div>
	);
}

AuditLog = connect((state) => {
	return {
		...state.auditLog,
		loading: state.loading.effects['auditLog/getAuditLog']
	};
})(AuditLog);

export default create()(AuditLog);
