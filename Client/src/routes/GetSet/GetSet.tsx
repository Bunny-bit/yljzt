import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Table, Button, Input, Modal, Checkbox, Tag, Row, Col, Badge } from 'antd';
const { Column, ColumnGroup } = Table;
const Search = Input.Search;
const confirm = Modal.confirm;
import { remoteUrl } from '../../utils/url';
import moment from 'moment';

class GetSet extends React.Component {
	componentDidMount() {
		this.props.dispatch({
			type: 'getSet/get',
			payload: {
				api: this.props.getApi
			}
		});
	}
	render() {
		const { dispatch, data, form, formNode, setApi, getApi } = this.props;
		return (
			<div>
				{formNode}
				<center>
					<Button
						key="back"
						type="primary"
						onClick={() => {
							form.validateFields((err, values) => {
								if (!err) {
									dispatch({
										type: 'getSet/set',
										payload: {
											data: values,
											api: setApi
										}
									});
								}
							});
						}}
					>
						保存
					</Button>
				</center>
			</div>
		);
	}
}
GetSet = connect((state) => {
	return {
		...state.getSet
	};
})(GetSet);

export default GetSet;
