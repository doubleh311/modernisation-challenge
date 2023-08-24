<script setup>
  import { ref, reactive, onMounted } from 'vue'
  import axios from 'axios';
  import snackbar from './snackbar';

const newItem = {
  id: 0, details: '', completed: false, dateCreated: null, dateModified: null, dateDeleted: null, showMenu: false
}
const modalActive = ref(false);
const activeMenuId = ref(-1);
const activeMenuElement = ref(null);
const editingItem = ref(newItem);

const data = reactive({tasks: []})

onMounted(() => {
  document.addEventListener("click", (e) => {
    if (!e.target.matches(".popup_menu") && activeMenuId.value != -1) {
      activeMenuElement.value.style.display = 'none';
      activeMenuId.value = -1;
      activeMenuElement.value = null;
    }
  });

  axios.get('https://localhost:7038/api/Task')
  .then(function (response) {
    data.tasks = response.data.map(x => ({...x, showMenu: false}))
  })
  .catch(function (error) {
    console.log(error);
  });
})


const toggleMenu = (id, event) => {
  switch (id) {
    case -1: {
      activeMenuId.value = id;
      activeMenuElement.value = event.target.querySelector('span');
      activeMenuElement.value.style.display = 'inline-block';
      break; }
    case activeMenuId.value: {
      activeMenuElement.value.style.display = 'none';
      activeMenuId.value = -1;
      activeMenuElement.value = null;
      break;
    }
    default: {
      activeMenuId.value = id;
      if (activeMenuElement.value != null) {
        activeMenuElement.value.style.display = 'none';
      }
      activeMenuElement.value = event.target.querySelector('span');
      activeMenuElement.value.style.display = 'inline-block';
      break;
    }
  }
}

const toggleCreateModal = () => {
  editingItem.value = {...newItem};
  modalActive.value = !modalActive.value;
}

const createItem = (id) => {
  if (editingItem.value.details.length == 0) {
    snackbar("error", "One or more required fields haven't been filled in.");
    return;
  }
  axios.post('https://localhost:7038/api/Task', editingItem.value)
  .then(function (response) {
    data.tasks.push(response.data)
  })
  .catch(function (error) {
    console.log(error);
    snackbar("error", "One or more required fields haven't been filled in.");
  })
  .finally(function () {
    editingItem.value = {...newItem};
  });
  modalActive.value = !modalActive.value;
}

const editItem = (id) => {
  let item = data.tasks.find(x => x.id == id);
  if (item) {
    editingItem.value = {...item};
  }
  modalActive.value = !modalActive.value;
}

const saveItem = () => {
  if (editingItem.value.details.length == 0) {
    snackbar("error", "One or more required fields haven't been filled in.");
    return;
  }
  axios.put('https://localhost:7038/api/Task', editingItem.value)
    .then(function (response) {
      data.tasks = data.tasks.map(x => {
        if (x.id == response.data.id) {
          return response.data;
        } else {
          return x;
        }
      })
    })
    .catch(function (error) {
      console.log(error);
      snackbar("error", "One or more required fields haven't been filled in.");
    })
    .finally(function () {
      editingItem.value = {...newItem};
    });
  modalActive.value = !modalActive.value;
}

const deleteItem = (id) => {
  if (window.confirm("Are you sure that you want to delete this task?")) {
    let item = data.tasks.find(x => x.id == id);
    if (item) {
      axios.delete(`https://localhost:7038/api/Task?id=${id}`)
      .then(function (response) {
        data.tasks = data.tasks.filter(x => x.id != id);
      })
      .catch(function (error) {
        console.log(error);
      });
    }
  }
}

const completeItem = (id) => {
  let item = data.tasks.find(x => x.id == id);
  if (item) {
    item.completed = !item.completed;
    axios.patch(`https://localhost:7038/api/Task?id=${id}`)
      .then(function (response) {
        data.tasks = data.tasks.map(x => {
        if (x.id == response.data.id) {
          return response.data;
        } else {
          return x;
        }
      })
      })
      .catch(function (error) {
        console.log(error);
      });
  }
}

</script>

<template>
  <main class="main">
    <h1>Tasks</h1>
    <div>
      <div class="table">
        <table>
          <thead>
            <tr>
                <th style="width: 1px;">Completed</th>
                <th>Details</th>
                <th style="width: 1px;"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item of data.tasks" :key="`todo_task_${item.id}`">
              <td style="text-align: center; width: 1px;">
                <span class="checkbox">
                  <input 
                    :id="item.id"
                    type="checkbox" 
                    name="ctl00$BodyContentPlaceHolder$TasksRepeater$ctl00$CompletedCheckBox" 
                    :checked="item.completed" 
                    @click="completeItem(item.id)">
                </span>
              </td>
              <td>{{item.details}}</td>
              <td style="width: 1px;">
                  <span class="popup_menu" :id="`popup_menu_span_${item.id}`" @click="toggleMenu(item.id, $event)">
                      <span :style="{ 'display': item.showMenu ? 'inline-block' : 'none' }">
                          <a @click="editItem(item.id)">Edit</a>
                          <a @click="deleteItem(item.id)">Delete</a>
                      </span>
                  </span>
              </td>
            </tr>
            
            <tr class="info">
                <td colspan="99">
                    <a @click="toggleCreateModal()" id="BodyContentPlaceHolder_CreateLinkButton">+ Create a new task</a>
                </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div id="BodyContentPlaceHolder_ctl01" v-if="modalActive">
      <div class="dialogue">
        <div style="width: 750px;">
          <div class="header">
            <a id="BodyContentPlaceHolder_CloseLinkButton" class="close" @click="toggleCreateModal()"></a>
            <h2>
                {{ editingItem.id == 0 ? 'Create task' : 'Edit task' }}
            </h2>
          </div>
          <div class="body">
              <fieldset class="required">
                  <label>Details</label>
                  <textarea 
                    name="ctl00$BodyContentPlaceHolder$DetailsTextBox" rows="2" cols="20" 
                    id="BodyContentPlaceHolder_DetailsTextBox" class="text" style="height:100px;"
                    v-model="editingItem.details"
                  ></textarea>
              </fieldset>
          </div>
            <div class="footer">
              <p class="commands">
                <span class="grow"></span>
                <a id="BodyContentPlaceHolder_CancelLinkButton" class="button hollow" @click="toggleCreateModal()">Cancel</a>
                <a id="BodyContentPlaceHolder_SaveLinkButton" class="button" @click="editingItem.id == 0 ? createItem() : saveItem()">Save</a>
              </p>
            </div>
        </div>
      </div>
    </div>
  </main>
</template>

<style type="text/css">
#BodyContentPlaceHolder_CreateLinkButton:hover {
  cursor:pointer;
 }
</style>