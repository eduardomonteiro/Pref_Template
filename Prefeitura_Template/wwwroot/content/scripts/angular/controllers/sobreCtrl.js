app.controller('sobreController', function($scope, Cidade){
 

    $scope.tabActive = 1;
    $scope.showContent = 1;

    $scope.changeTab = function(index) {

        $scope.showContent = index;
        $scope.tabActive = index;
    };

    Cidade._getDados()
    .then(function successCallback(response) {

        $scope.conteudo = response.data;

        $scope.timeline = response.data.Timeline;

        $scope.slickConfig = {
            dots: true,
            arrows: false,
            slidesToShow: 3,
            infinite: false,
            slidesToScroll: 1,
            arrows: true,
            nextArrow: '<button class="fa arrow fa-angle-right"></button>',
            prevArrow: '<button class="fa arrow fa-angle-left"></button>',
            responsive: [
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        dots: false
                    }
                }
            ]
        };  
    });

});