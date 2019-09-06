import React from 'react';
import styles from './../User.css';
import { Checkbox, Button, Popover, Icon, Row, Col } from 'antd';
const CheckboxGroup = Checkbox.Group;
import { connect } from 'dva';

/**
 * CustomColumnSelector.js
 * Created by 凡尧 on 2017/9/19
 * 描述: 用户管理界面自定义条目
 */
function CustomColumnSelector({ dispatch, visibleColumnTitles, customColumnSelectorVisible }) {
	function onChange(value) {
		dispatch({
			type: 'user/setState',
			payload: {
				visibleColumnTitles: value,
				visibleColumnWidth: value.length * 100 + 200
			}
		});
	}

	function resetColumns() {
		dispatch({
			type: 'user/setState',
			payload: {
				visibleColumnTitles: [
					'用户名',
					'用户编号',
					'姓名',
					'角色',
					'邮箱地址',
					'手机号',
					'邮箱地址验证',
					'手机号码验证',
					'启用',
					'锁定',
					'上次登录时间',
					'创建时间',
					'操作'
				],
				visibleColumnWidth: 1500
			}
		});
	}

	const customColumnSelector = (
		<div>
			<div style={{ borderBottom: 1, width: 220 }}>
				<CheckboxGroup value={visibleColumnTitles} onChange={onChange}>
					<Row>
						<Col span={12}>
							<Checkbox value="用户编号">用户编号</Checkbox>
						</Col>
						<Col span={12}>
							<Checkbox value="姓名">姓名</Checkbox>
						</Col>
						<Col span={12}>
							<Checkbox value="角色">角色</Checkbox>
						</Col>
						<Col span={12}>
							<Checkbox value="邮箱地址">邮箱地址</Checkbox>
						</Col>
						<Col span={12}>
							<Checkbox value="手机号">手机号</Checkbox>
						</Col>
						<Col span={12}>
							<Checkbox value="邮箱地址验证">邮箱地址验证</Checkbox>
						</Col>
						<Col span={12}>
							<Checkbox value="手机号码验证">手机号码验证</Checkbox>
						</Col>
						<Col span={12}>
							<Checkbox value="启用">启用</Checkbox>
						</Col>
						<Col span={12}>
							<Checkbox value="锁定">锁定</Checkbox>
						</Col>
						<Col span={12}>
							<Checkbox value="上次登录时间">上次登录时间</Checkbox>
						</Col>
						<Col span={12}>
							<Checkbox value="创建时间">创建时间</Checkbox>
						</Col>
					</Row>
				</CheckboxGroup>
			</div>
			<div style={{ textAlign: 'center' }}>
				<a onClick={hide}>关闭</a>
			</div>
		</div>
	);
	const selectorTitle = (
		<div>
			<span>列表显示条目</span>
			<a onClick={resetColumns} style={{ float: 'right' }}>
				恢复默认
			</a>
		</div>
	);

	function show() {
		dispatch({
			type: 'user/setState',
			payload: {
				customColumnSelectorVisible: true
			}
		});
	}

	function hide() {
		dispatch({
			type: 'user/setState',
			payload: {
				customColumnSelectorVisible: false
			}
		});
	}

	function handleVisibleChange(visible) {
		dispatch({
			type: 'user/setState',
			payload: {
				customColumnSelectorVisible: visible
			}
		});
	}

	return (
		<Popover
			content={customColumnSelector}
			title={selectorTitle}
			placement="bottomRight"
			visible={customColumnSelectorVisible}
			trigger="click"
			onVisibleChange={handleVisibleChange}
		>
			<Button style={{ float: 'right', marginRight: 0 }} onClick={show}>
				自定义列表条目<Icon type="down" />
			</Button>
		</Popover>
	);
}

CustomColumnSelector = connect((state) => {
	return {
		...state.user
	};
})(CustomColumnSelector);

export default CustomColumnSelector;
