app.directive('fontResize', function(){

    var limit = 14;
    
    function addSize(limit) {
        $("body").css('font-size', limit);
    };

    return  {
        restrict: 'A',
        link: function(scope, elem, attrs) {
            
           scope.plus = function() {

                limit < 14 ? limit++ : limit = 14; 
                addSize(limit);
           };
           scope.minus = function() {

                limit == 10 || limit > 8 ? limit-- : limit = 10; 
                addSize(limit);
            };
        }
    };
});


app.directive('acordion', function(){
    
    return  {
        restrict: 'A',
        link: function(scope, elem, attrs) {
            
            scope.open = function() {
                
                var content = elem.find('.content');

                elem.toggleClass('active');

                content.slideToggle();
                
            };

        }
    };
});

app.directive('routeLoadingIndicator', function($rootScope, $timeout) {
    return {
      restrict: 'E',
      template: "<div ng-show='isRouteLoading' class='sk-circle absolute'>" +
      "<div class='sk-circle1 sk-child'>" +"</div>" +
      "<div class='sk-circle2 sk-child'>" +"</div>" +
      "<div class='sk-circle3 sk-child'>" +"</div>" +
      "<div class='sk-circle4 sk-child'>" +"</div>" +
      "<div class='sk-circle5 sk-child'>" +"</div>" +
      "<div class='sk-circle6 sk-child'>" +"</div>" +
      "<div class='sk-circle7 sk-child'>" +"</div>" +
      "<div class='sk-circle8 sk-child'>" +"</div>" +
      "<div class='sk-circle9 sk-child'>" +"</div>" +
      "<div class='sk-circle10 sk-child'>" +"</div>" +
      "<div class='sk-circle11 sk-child'>" +"</div>" +
      "<div class='sk-circle12 sk-child'>" +"</div>" +
      "</div>",
      replace: true,
      link: function(scope, elem, attrs) {
   
        $rootScope.$on('$routeChangeStart', function() {
          scope.isRouteLoading = true;
            $('body').animate({
            scrollTop: 0
            }, 400);
        });
        
        $rootScope.$on('$routeChangeSuccess', function() {

          scope.isRouteLoading = true;

          $timeout(function(){

            scope.isRouteLoading = false;

          }, 2000);
          
        });
      }
    };
});
    

