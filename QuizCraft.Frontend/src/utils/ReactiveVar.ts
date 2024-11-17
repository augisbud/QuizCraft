type Subscriber<T> = (value: T) => void;

export class ReactiveVar<T> {
  private value: T;
  private subscribers: Set<Subscriber<T>> = new Set(); // Use Set to manage unique subscribers

  constructor(initialValue: T) {
    this.value = initialValue;
  }

  get(): T {
    return this.value;
  }

  set(newValue: T): void {
    if (this.value !== newValue) {
      this.value = newValue;
      this.notify();
    }
  }

  subscribe(subscriber: Subscriber<T>): () => void {
    this.subscribers.add(subscriber);

    // Return an unsubscribe function
    return () => {
      this.subscribers.delete(subscriber);
    };
  }

  private notify(): void {
    this.subscribers.forEach((subscriber) => subscriber(this.value));
  }
}

// Factory to create a ReactiveVar
export const makeVar = <T>(initialValue: T): ReactiveVar<T> => new ReactiveVar<T>(initialValue);
