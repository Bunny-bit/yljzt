import React from 'react';
import { Form, Checkbox, Input, DatePicker, InputNumber } from 'antd';
const create = Form.create;
const FormItem = Form.Item;

class Setting extends React.Component {
	onChangeHandle = () => {
		setTimeout(() => {
			this.props.form.validateFields((err, values) => {
				this.props.onChange(values);
			});
		});
	};
	componentWillMount() {
		this.onChangeHandle();
	}
	componentDidUpdate(prevProps, prevState) {
		if (JSON.stringify(this.props.data) != JSON.stringify(prevProps.data)) {
			this.onChangeHandle();
		}
	}
	render() {
		const { getFieldDecorator } = this.props.form;
		const { data = [] } = this.props;
		return (
			<Form layout="horizontal" className="ant-advanced-search-form">
				{data.map((n, i) => (
					<div key={n.name}>
						{n.title ? <h2>{n.title}</h2> : ''}
						{((n) => {
							switch (n.type) {
								case 'boolean':
									return (
										<FormItem help={n.description}>
											{getFieldDecorator(n.name, {
												initialValue: n.value,
												valuePropName: 'checked'
											})(<Checkbox onChange={this.onChangeHandle}>{n.displayName}</Checkbox>)}
										</FormItem>
									);
								case 'datetime':
									return (
										<FormItem help={n.description} label={n.displayName}>
											{getFieldDecorator(n.name, {
												initialValue: n.value
											})(<DatePicker onChange={this.onChangeHandle} />)}
										</FormItem>
									);
								case 'int32':
								case 'single':
								case 'decimal':
								case 'double':
									return (
										<FormItem help={n.description} label={n.displayName}>
											{getFieldDecorator(n.name, {
												initialValue: n.value
											})(<InputNumber onChange={this.onChangeHandle} />)}
										</FormItem>
									);
								default:
									return (
										<FormItem help={n.description} label={n.displayName}>
											{getFieldDecorator(n.name, {
												initialValue: n.value
											})(<Input onChange={this.onChangeHandle} />)}
										</FormItem>
									);
							}
						})(n)}
					</div>
				))}
			</Form>
		);
	}
}

export default create()(Setting);
