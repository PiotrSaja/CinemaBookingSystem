<template>
  <section class="layout">
    <div class="sidebar"><Navbar /></div>
    <div class="body">
      <div class="row">
        <Menubar1 />
      </div>
      <div class="row p-2">
        <div>
          <ConfirmDialog></ConfirmDialog>
          <DataTable
            :value="movies"
            :paginator="true"
            class="p-datatable-customers"
            :rows="10"
            dataKey="id"
            :rowHover="true"
            v-model:selection="selectedMovies"
            v-model:filters="filters"
            filterDisplay="menu"
            :loading="loading"
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
            :rowsPerPageOptions="[10, 25, 50]"
            currentPageReportTemplate="Showing {first} to {last} of {totalRecords} entries"
            :globalFilterFields="['title', 'plot']"
            responsiveLayout="scroll"
          >
            <template #header>
              <div class="flex justify-content-left align-items-center">
                <h5 class="m-0">Movies</h5>
                <span class="p-input-icon-left ml-5">
                  <i class="pi pi-search" />
                  <InputText
                    v-model="filters['global'].value"
                    placeholder="Keyword Search"
                  />
                </span>
              </div>
            </template>
            <template #empty> No movies found. </template>
            <template #loading> Loading movies data. Please wait. </template>
            <Column header="Poster">
              <template #body="{ data }">
                <img
                  :src="data.posterPath"
                  :alt="data.posterPath"
                  class="movie-image"
                />
              </template>
            </Column>
            <Column
              field="title"
              header="Title"
              sortable
              style="min-width: 14rem"
            >
              <template #body="{ data }">
                {{ data.title }}
              </template>
              <template #filter="{ filterModel }">
                <InputText
                  type="text"
                  v-model="filterModel.value"
                  class="p-column-filter"
                  placeholder="Search by title"
                />
              </template>
            </Column>
            <Column
              field="plot"
              header="Plot"
              sortable
              filterMatchMode="contains"
              style="min-width: 14rem"
            >
              <template #body="{ data }">
                <span>{{ data.plot }}</span>
              </template>
              <template #filter="{ filterModel }">
                <InputText
                  type="text"
                  v-model="filterModel.value"
                  class="p-column-filter"
                  placeholder="Search by plot"
                />
              </template>
            </Column>
            <Column
              headerStyle="width: 4rem; text-align: center"
              bodyStyle="text-align: center; overflow: visible"
            >
              <template #body="{ data }">
                <Button
                  type="button"
                  icon="pi pi-trash"
                  class="p-button-danger"
                  @click="deleteMovie(data.id)"
                ></Button>
              </template>
            </Column>
          </DataTable>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Menubar1 from "@/components/Menubar.vue";
import { FilterMatchMode, FilterOperator } from "primevue/api";
import MovieService from "@/services/movie-service";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import InputText from "primevue/inputtext";
export default {
  name: "MoviesView",
  components: {
    DataTable,
    Column,
    Button,
    InputText,
    Navbar,
    Menubar1,
  },
  data() {
    return {
      movies: null,
      selectedMovies: null,
      filters: {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        title: {
          operator: FilterOperator.AND,
          constraints: [
            { value: null, matchMode: FilterMatchMode.STARTS_WITH },
          ],
        },
        plot: {
          operator: FilterOperator.AND,
          constraints: [
            { value: null, matchMode: FilterMatchMode.STARTS_WITH },
          ],
        },
      },
      loading: true,
    };
  },
  productService: null,
  created() {
    MovieService.getAll(1, 1000000)
      .then((response) => {
        this.movies = response.data.items;
        this.loading = false;
      })
      .catch((error) => {
        console.log(error.response.data);
      });
  },
  methods: {
    initFilters() {
      this.filters = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
      };
    },
    deleteMovie(id) {
      MovieService.delete(id)
        .then((response) => {
          console.log(response.data);
          this.getMovieList();
        })
        .catch((error) => {
          console.log(error.response.data);
        });
    },
    getMovieList() {
      MovieService.getAll(1, 1000000)
        .then((response) => {
          this.movies = response.data.items;
          this.loading = false;
        })
        .catch((error) => {
          console.log(error.response.data);
        });
    },
  },
};
</script>

<style lang="scss"  scoped>
::v-deep(.p-paginator) {
  .p-paginator-current {
    margin-left: auto;
  }
}

::v-deep(.p-progressbar) {
  height: 0.5rem;
  background-color: #d8dadc;

  .p-progressbar-value {
    background-color: #607d8b;
  }
}

::v-deep(.p-datepicker) {
  min-width: 25rem;

  td {
    font-weight: 400;
  }
}

::v-deep(.p-datatable.p-datatable-customers) {
  .p-datatable-header {
    padding: 1rem;
    text-align: left;
    font-size: 1.5rem;
  }

  .p-paginator {
    padding: 1rem;
  }

  .p-datatable-thead > tr > th {
    text-align: left;
  }

  .p-datatable-tbody > tr > td {
    cursor: auto;
  }

  .p-dropdown-label:not(.p-placeholder) {
    text-transform: uppercase;
  }
  .movie-image {
    width: 150px;
    box-shadow: 0 3px 6px rgba(0, 0, 0, 0.16), 0 3px 6px rgba(0, 0, 0, 0.23);
  }
}
[v-cloak] {
  display: none;
}
.layout {
  width: 100%;
  display: grid;
  grid:
    "sidebar body" 1fr
    / 15% 85%;
  gap: 8px;
}
.sidebar {
  grid-area: sidebar;
  min-height: 100vh;
  border-right: 1px solid lightgray;
}
.body {
  grid-area: body;
}
</style>