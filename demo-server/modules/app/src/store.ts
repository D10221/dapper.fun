import { createStore, combineReducers, applyMiddleware, compose as reduxCompose } from "redux";
import thunk from "redux-thunk";
import * as user from "./user";
/** */
const reducers = {
    root: (state: any = {}, _action: any) => state,
    [user.STORE_KEY]: user.reducer,
};
/** */
const compose = (process.env.NODE_ENV !== "production" && (window as any).__REDUX_DEVTOOLS_EXTENSION_COMPOSE__) || reduxCompose;
/** */
const store = createStore(
    combineReducers(reducers),
    compose(
        applyMiddleware(
            thunk,           
        )
    )
)
export default store;