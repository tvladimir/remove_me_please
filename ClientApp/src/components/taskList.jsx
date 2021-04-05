import React, { Component } from "react";
import taskService from '../services/taskService';
import { Button, Paper, Typography, flexbox } from '@material-ui/core';
import TaskListItem from '../components/taskListItem';

class TaskList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            errors: {},
        }
        this.loadTasks = this.loadTasks.bind(this);
    }

    async loadTasks() {
        try {
            const { data } = await taskService.getAllTasks();
            if (data.length > 0) {
                this.props.handleServerUpdate(data);
            }
        } catch (err) {
           if (err.response && err.response.status === 401) {
           this.setState({ errors: { tasks: "No tasks to show" } });
        }
    }

}
    render() {
        const tasks  = this.props.tasks;
        return (
            <div style={{ width: '50%', display: 'flex', }}>
                <Paper style={{ padding: 16, width: '100%',  height: '50vh', display:'flex', 'flexDirection': 'column', overflow:'auto'}} >
                <Typography variant="h6" className="list-header">
                    Current Tasks
          </Typography>
                {tasks.map(task => (
                    <TaskListItem key={task.id} task={task} />
                ))}
                    <Button color="primary" variant="contained" onClick={this.loadTasks} style={{
                        'alignItems': 'flex-end', 'justifyContent': 'center', 'alignSelf': 'center', 'position': 'relative',
                        'bottom': '6px' }}>Load All</Button>
                </Paper>
            </div>
        )
    }
}
export default TaskList;
