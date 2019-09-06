import React, { Component } from 'react';
import { connect } from 'dva';
import { Button, message, Tooltip, Modal, Input, Form } from 'antd';
const Search = Input.Search;

class ReadCPUCard extends Component {
	static defaultProps = {
		text: '读取IC卡信息',
		errorText: '控件不可用，可能未正确安装控件及驱动，或者控件未启用。'
	};

	state = {
		error: false,
		loading: false
	};

	componentDidMount() {
		this.props.dispatch({
			type: 'cloudServer/checkCPUCardReady',
			payload: {},
			callBack: (data) => {
				if (!data || data[0].status != 200) {
					this.setState({ error: true });
					Modal.error({ content: this.props.errorText });
				}
			}
		});
	}

	render() {
		const { form, onRead, onChange, text, style, errorText, value } = this.props;
		const { error, loading } = this.state;
		return (
			<div style={style}>
				<Button
					disabled={error}
					loading={loading}
					type="primary"
					onClick={() => {
						this.setState({ loading: true });
						this.props.dispatch({
							type: 'cloudServer/getCPUCard',
							payload: {},
							callBack: (data) => {
								this.setState({ loading: false });
								if (data.resultFlag != 0) {
									message.error(data.errorMsg);
									return;
								}
								let value = data.resultContent.cardNumber;
								form.setFieldsValue({ input: value });
								if (onRead) {
									onRead(value);
								}
								if (onChange) {
									onChange(value);
								}
							}
						});
					}}
				>
					{text}
				</Button>
				<span>
					卡号：
					{form.getFieldDecorator('input')(
						<Search
							style={{ width: '140px' }}
							onSearch={(value) => {
								if (onRead) {
									onRead(value);
								}
							}}
							onChange={(value) => {
								if (onChange) {
									onChange(value);
								}
							}}
						/>
					)}
				</span>
			</div>
		);
	}
}

ReadCPUCard = connect((state) => {
	return {
		...state.cloudServer
	};
})(ReadCPUCard);
export default Form.create()(ReadCPUCard);
