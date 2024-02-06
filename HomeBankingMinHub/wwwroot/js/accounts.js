var app = new Vue({
    el:"#app",
    data:{
        clientInfo: {},
        error: null
    },
    methods:{
        getData: function(){
            axios.get("/api/clients/4")
            .then(function (response) {
                //get client ifo
                app.clientInfo = response.data;
                console.log(app.clientInfo);
            })
            .catch(function (error) {
                // handle error
                app.error = error;
            })
        },
        formatDate: function(date){
            return new Date(date).toLocaleDateString('en-gb');
        }
    },
    mounted: function(){
        this.getData();
    }
})