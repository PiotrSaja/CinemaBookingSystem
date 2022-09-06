<template>
  <div class="container page text-white">
    <div class="background-image" style="background-image: url('https://wallpaperaccess.com/full/1773889.jpg')">
    </div>
    <h5 class="underline font-weight-bold mt-4">All cinemas</h5>
    <div class="row mb-3">
      <div class="col-md-4 col-12" v-for="cinema in cinemas.items"
                  :key="cinema.id"
                  :cinema="cinema" style="cursor: pointer;" @click="onCinemaClicked(cinema.id)">
          <div class="row text-white mt-4 background">
          <div class="col-md-12 mt-5 mb-4">
              <h4 style="cursor: pointer;">{{ cinema.name}}</h4>
              <h6>City: {{ cinema.city }}</h6>
              <h6>Street: {{ cinema.street }}</h6><br>
          </div>
          <hr>
          </div>
      </div>
    </div>
  </div>
</template>

<script>
import CinemaService from '@/api-services/cinema-service'
export default {
  name: 'Cinemas',
  data () {
    return {
      cinemas: {}
    }
  },
  created () {
    CinemaService.getAll().then((response) => {
      this.cinemas = response.data
    }).catch((error) => {
      console.log(error.response.data)
    })
  },
  methods: {
    showCinemaDetail (cinemaId) {
        this.$router.push({name: 'CinemaDetail', params: {id: cinemaId}})
      },
    onCinemaClicked (cinemaId) {
      this.showCinemaDetail(cinemaId)
    }
  }
}
</script>

<style scoped>
.page {
min-height: 100vh;
}
.background-image{
  height: 350px;
  min-height: 350px;
  width: 100%;
  background-repeat: no-repeat;
  background-position: bottom center;
  background-size: cover;
  -webkit-mask-image: -webkit-gradient(linear, left top, left bottom, color-stop(0, black), color-stop(0.35, black), color-stop(0.5, black), color-stop(0.65, black), color-stop(0.85, rgba(0, 0, 0, 0.6)), color-stop(1, transparent));
}
.image-wrapper {
  height:240px;
  display:flex;
  flex-direction:column;
  justify-content:center;
  text-align:center;
  box-shadow:  0 0 3px rgba(0,0,0,0.3);
 }
 .background{
   text-align: center;
   background-color: gray;
   margin: 3px;
   border-radius: 3px;
   transition: 0.5s;
}
.background:hover{
   background-color: #FF9100;
}
.underline {
  margin-top: 15px;
  margin-bottom: 30px;
  text-align: center;
  width:150px;
  border-bottom: 2px solid #FF9100;
  position: relative;
  left: 50%;
  transform: translateX(-50%);
}
</style>
