<template>
<div class="movie-grid" @click="onMovieClicked(movieItem.id)">
  <div class="movie-image">
    <a href="" class="image">
      <img class="pic-1" :src="movieItem.posterPath" />
    </a>
    <span class="movie-label" v-if="movieItem.imdbRating >= 7.5">Mega hit!</span>
    <div class="movie-content">
    <h3 class="title"><a href="#">{{ movieItem.title }}</a></h3>
  </div>
  </div>
</div>
</template>

<script>
export default {
  name: 'MovieItem',
  props: {
    movieItem: Object
  },
  data () {
    return {
      count: 0
    }
  },
  methods: {
      onMovieClicked (movieId) {
        this.$router.push({name: 'MovieDetail', params: {id: movieId}})
      }
  }
}
</script>

<style scoped lang="css">
.movie-grid{
  font-family: 'Montserrat', sans-serif;
  text-align: center;
  margin: 25px 8px 0;
  border-radius: 10px;
  box-shadow:  0 0 3px rgba(0,0,0,0.1);
  transition: all 0.5s;
}
.movie-grid .movie-image img{
  width: 100%;
  height: 315px;
}
@media only screen and (min-width: 1px) and (max-width: 576px) {
  .movie-grid {
    font-family: 'Montserrat', sans-serif;
    text-align: center;
    margin: 15px 0px 0;
    border-radius: 10px;
    box-shadow:  0 0 3px rgba(0,0,0,0.1);
    transition: all 0.5s;
  }
  .movie-grid .movie-image img{
    width: 100%;
    height: 215px;
}
}
.movie-grid:hover{
  box-shadow:  0 0 30px rgba(0,0,0,0.4);
  transform: scale(1.05);
}
.movie-grid .movie-image{ position: relative; }
.movie-grid .movie-image a.image{
    border-radius: 10px 10px 0 0;
    overflow: hidden;
    display: block;
}
.movie-grid .movie-label{
    color: #fff;
    background: #FF5555;
    font-size: 13px;
    text-transform: uppercase;
    padding: 2px 6px;
    border-radius: 3px;
    position: absolute;
    left: 50%;
    transform: translateX(-50%);
    top: -3px;
}
.movie-grid .movie-content{
    background-color: #fff;
    text-align: left;
    padding: 5px 5px;
    border-top: 1px solid transparent;
    border-radius: 0 0 10px 10px;
    transition: all 0.3s;
    min-height: 67px;
    display: flex;
    justify-content: center; /* align horizontal */
    align-items: center; /* align vertical */
}
.movie-grid:hover .movie-content{ border-top-color: #dbdbdb; }
.movie-grid .title{
    text-align: center;
    font-size: 15.5px;
    font-weight: bold;
    text-transform: capitalize;
    margin: 0;
}
.movie-grid .title a{
    color: #323b45;
    transition: all 0.4s ease-out;
}
.movie-grid .title a:hover{ color: #FF9100; }
@media screen and (min-width: 577px) and (max-width:990px){
    .movie-grid{ margin: 5px 15px 30px; }
}
</style>
