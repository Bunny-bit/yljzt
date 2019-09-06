import React from 'react';

import { Cascader } from 'antd';
import Card from './Card';
import update from 'immutability-helper';
import { DragDropContext } from 'react-dnd';
import HTML5Backend from 'react-dnd-html5-backend';

class SortableCard extends React.Component {
	state = {
		oldData: [],
		data: [],
		type: ''
	};
	componentDidMount() {
		this.setState({ data: this.props.data, oldData: this.props.data, type: Math.random() + '' });
	}
	componentDidUpdate(prevProps, prevState) {
		if (JSON.stringify(this.props.data) != JSON.stringify(prevProps.data)) {
			this.setState({ data: this.props.data, oldData: this.props.data });
		}
	}
	render() {
		const { rander = (n, i) => {}, onChange = (data) => {}, cardStyle = {} } = this.props;
		const { data = [], type } = this.state;
		let _this = this;
		function moveCard(dragIndex, hoverIndex) {
			const dragCard = data[dragIndex];
			let _data = update([ ...data ], {
				$splice: [ [ dragIndex, 1 ], [ hoverIndex, 0, dragCard ] ]
			});
			_this.setState({
				data: _data
			});
			onChange(_data);
		}
		return (
			<div>
				{data.map((n, i) => (
					<Card index={i} key={n.id} id={n.id} moveCard={moveCard} type={type} cardStyle={cardStyle}>
						{rander(n, i)}
					</Card>
				))}
			</div>
		);
	}
}

export default DragDropContext(HTML5Backend)(SortableCard);
