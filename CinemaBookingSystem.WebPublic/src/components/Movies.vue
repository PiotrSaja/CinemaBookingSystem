<template>
  <div class="container page">
    <movie-carousel/>
    <div class="row mt-4">
        <div class="col-12 col-md-2">
            <b-form-input v-model="searchString" @keydown.native="fetchData()" placeholder="Search movie" class="mt-2 mr-5"></b-form-input>
        </div>
        <div class="col-12 col-md-8">
        <h5 class="text-white underline font-weight-bold">Movies</h5>
        </div>
        <div class="col-12 col-md-2 hideRecordsNumberOnMobile">
            <b-dropdown text="Records number" class="mt-2 mr-5">
            <b-dropdown-item @click="updateLimitOnPage(8)">8</b-dropdown-item>
            <b-dropdown-item @click="updateLimitOnPage(16)">16</b-dropdown-item>
            <b-dropdown-item @click="updateLimitOnPage(32)">32</b-dropdown-item>
            </b-dropdown>
        </div>
    </div>
    <div class="container">
      <div class="row">
        <div class="col-xl-3 col-lg-4 col-md-6 col-sm-6 col-6 padding"
        v-for="movie in movies.items"
        :key="movie.id"
        :movie="movie"
        >
          <movie-item :movieItem="movie"></movie-item>
        </div>
      </div>
    </div>
    <b-pagination
      @input="fetchData()"
      v-model="currentPage"
      :total-rows="movies.totalItems"
      :per-page="limitOnPage"
      size="sm"
      align="center"
      hide-goto-end-buttons
      pills
      class="mt-5 custom-pagination"
    ></b-pagination>
  </div>
</template>

<script>
import VueResizeText from 'vue-resize-text'
import MovieService from '@/api-services/movie-service'
import MovieCarousel from '@/components/MovieCarousel'
import MovieItem from '@/components/MovieItem'
export default {
  components: { MovieCarousel, VueResizeText, MovieItem },
  name: 'Movies',
  data () {
    return {
      movies: {},
      limitOnPage: 16,
      currentPage: 1,
      searchString: ''
    }
  },
  created () {
   this.fetchData()
  },
  methods: {
      fetchData () {
          MovieService.getAll(this.currentPage, this.limitOnPage, this.searchString).then((response) => {
            this.movies = response.data
        })
      },
      updateLimitOnPage (number) {
          this.limitOnPage = number
          this.fetchData()
      }
  }
}
</script>

<style scoped lang="css">
  .padding {
    padding-right: 10px;
    padding-left: 10px;
  }
  .container, .container-fluid, .container-lg, .container-md, .container-sm, .container-xl /deep/{
    padding-right: 0px;
    padding-left: 0px;
    margin-right: auto;
    margin-left: auto;
  }

  .hideRecordsNumberOnMobile {
    display: none
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
    padding-bottom:10px
  }
  .page {
  min-height: 100vh;
  }
  .custom-pagination /deep/ .page-link {
    color: gray;
  }
  .custom-pagination /deep/ .page-item {
    color: white;
  }
  .custom-pagination /deep/ .page-item.active .page-link {
    background-color: #FF9100;
    border-color: #FF9100;
    color: white;
  }
</style>
