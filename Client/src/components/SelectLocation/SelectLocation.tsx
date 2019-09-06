import React, { Component } from 'react';

export default class SelectLocation extends Component {
	static defaultProps = {
		width: '100%',
		height: '400px'
	};

	state = {
		map: {},
		isInit: false
	};

	componentDidMount() {
		let BMap = window.BMap;
		let map = new BMap.Map('allmap', { enableMapClick: false });
		map.enableScrollWheelZoom(true);
		this.setState({ map: map });
	}

	componentDidUpdate(prevProps, prevState) {
		let _this = this;
		function onDragend(event) {
			var geoc = new BMap.Geocoder();
			geoc.getLocation(event.point, function(rs) {
				var addComp = rs.addressComponents;
				var address =
					addComp.province + addComp.city + addComp.district + addComp.street + addComp.streetNumber;
				_this.props.onChange({ longitude: event.point.lng, latitude: event.point.lat, name: address });
			});
		}

		let map = this.state.map;
		if (this.props.value && !this.state.isInit) {
			var point = new BMap.Point(this.props.value.longitude, this.props.value.latitude);
			map.centerAndZoom(point, 15);
			var marker = new BMap.Marker(point);
			map.addOverlay(marker);
			marker.enableDragging();
			marker.addEventListener('dragend', onDragend);

			this.setState({ isInit: true });
		}
	}

	render() {
		return (
			<div style={{ padding: '20px' }}>
				<div
					id="allmap"
					style={{
						width: this.props.width,
						height: this.props.height
					}}
				/>
			</div>
		);
	}
}
