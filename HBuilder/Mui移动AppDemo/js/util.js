     /*!
 * =====================================================
 * util v1.0 (http://www.kinfar.net)
 * =====================================================
 */
/**
 * 通用类
 * @type _L4.$|Function
 */
(function($, owner) {
	 $.ready(function() {
	 	var state = owner.getState();
		if (typeof(state) == undefined || state.token=="")
		{
			window.location.href="\login.html";
		}
	});

	/**
	 * 获取当前状态
	 **/
	owner.getState = function() {
		var stateText = localStorage.getItem('$state') || "{}";
		return JSON.parse(stateText);
	};

	/**
	 * 设置当前状态
	 **/
	owner.setState = function(state) {
		state = state || {};
		localStorage.setItem('$state', JSON.stringify(state));
		//var settings = owner.getSettings();
		//settings.gestures = '';
		//owner.setSettings(settings);
	};
}(mui, window.app = {}));